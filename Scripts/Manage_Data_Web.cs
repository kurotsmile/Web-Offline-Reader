using UnityEngine;
using UnityEngine.UI;

public class Manage_Data_Web : MonoBehaviour
{
    [Header("Obj App")]
    public Carrot.Carrot carrot;
    public View_Data data_view;

    [Header("Icon Asset")]
    public Sprite sp_icon_list_web;
    public Sprite sp_icon_web_dev;
    public Sprite sp_icon_web_brower;
    public Sprite sp_icon_list_link_on_site;
    public Sprite sp_icon_list_link_off_site;
    public Sprite sp_icon_web_view;

    private int length;
    private Carrot.Carrot_Box box_list_data;

    public void on_load()
    {
        this.length = PlayerPrefs.GetInt("length_data", 0);
    }

    public void add_data(string s_url,string s_data)
    {
        PlayerPrefs.SetString("data_" + this.length, s_data);
        PlayerPrefs.SetString("data_" + this.length + "_url", s_url);
        this.length++;
        PlayerPrefs.SetInt("length_data",this.length);
    }

    public void show_list_data_web()
    {
        this.box_list_data = this.carrot.Create_Box("list_data");
        box_list_data.set_title("List Data Web");

        for(int i = 0; i < this.length; i++)
        {
            string s_url = PlayerPrefs.GetString("data_" + i + "_url");

            if (s_url != "")
            {
                string s_data = PlayerPrefs.GetString("data_" + i);

                var index_web = i;
                var url_web = s_url;
                var data_web = s_data;

                Carrot.Carrot_Box_Item item_data = box_list_data.create_item("data_item_" + i);
                item_data.set_icon_white(sp_icon_web_dev);
                item_data.set_title(s_url);
                item_data.set_tip("Home page");
                item_data.set_act(() => show_data(index_web));

                Carrot.Carrot_Box_Btn_Item btn_view = item_data.create_item();
                btn_view.set_icon(this.sp_icon_web_view);
                btn_view.set_color(this.carrot.color_highlight);
                btn_view.set_act(() => show_data(index_web));

                Carrot.Carrot_Box_Btn_Item btn_list_link = item_data.create_item();
                btn_list_link.set_color(this.carrot.color_highlight);
                btn_list_link.set_icon(this.sp_icon_list_link_off_site);
                btn_list_link.set_act(() => this.data_view.show_list_link(data_web,false));

                Carrot.Carrot_Box_Btn_Item btn_brower = item_data.create_item();
                btn_brower.set_icon(this.sp_icon_web_brower);
                btn_brower.set_color(this.carrot.color_highlight);
                btn_brower.set_act(() => this.data_view.open_link(url_web));

                Carrot.Carrot_Box_Btn_Item btn_del = item_data.create_item();
                btn_del.set_icon(this.carrot.sp_icon_del_data);
                btn_del.set_color(this.carrot.color_highlight);
                btn_del.set_act(() => this.act_delete(index_web));
            }
        }
    }

    private void show_data(int index)
    {
        string s_url = PlayerPrefs.GetString("data_" + index + "_url");
        string s_data = PlayerPrefs.GetString("data_" + index);
        this.data_view.show(s_url,s_data);
        if (this.box_list_data != null) this.box_list_data.close();
    }

    public void act_delete(int index)
    {
        PlayerPrefs.DeleteKey("data_" + index + "_url");
        PlayerPrefs.DeleteKey("data_" + index);
        if (this.box_list_data != null) this.box_list_data.close();
    }

}
