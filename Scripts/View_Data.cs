using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Data : MonoBehaviour
{
    [Header("Obj main")]
    public Carrot.Carrot carrot;
    public Manage_Data_Web data_web;

    [Header("Ui")]
    public GameObject panel_view_data;
    public Text txt_title;
    public TextMeshProUGUI txt_data;

    [Header("Ui Emp")]
    public Image img_bt_code;
    public Image img_bt_text;
    public Image img_bk_editor;

    [Header("Color Set")]
    public Color32 color_nomal;
    public Color32 color_sel;

    public Color32 color_editor_text_bk;
    public Color32 color_editor_code_bk;

    public Color32 color_editor_text_txt;
    public Color32 color_editor_code_txt;

    private string s_data;
    private string s_url;
    private Carrot.Carrot_Box box_list_link;
    private bool is_view_code = false;

    public void on_load()
    {
        this.panel_view_data.SetActive(false);
    }

    public void show(string s_title,string s_data)
    {
        this.txt_title.text = s_title;
        this.s_url = s_title;
        this.s_data = s_data;
        this.panel_view_data.SetActive(true);

        this.is_view_code = false;
        this.check_model_view();
    }

    private void check_model_view()
    {
        this.reset_color_emp();
        if (this.is_view_code)
        {
            this.txt_data.text =this.s_data;
            this.img_bt_code.color = this.color_sel;
            this.img_bk_editor.color = this.color_editor_code_bk;
            this.txt_data.color = this.color_editor_code_txt;
        }
        else
        {
            this.txt_data.text = this.get_text(this.s_data);
            this.img_bt_text.color = this.color_sel;
            this.img_bk_editor.color = this.color_editor_text_bk;
            this.txt_data.color = this.color_editor_text_txt;
        }
    }

    public void btn_close()
    {
        this.panel_view_data.SetActive(false);
    }

    public List<string> get_list_url_off_site(string s_data)
    {
        List<string> list_url = new List<string>();

        string htmlCode = s_data;
        Regex linkRegex = new Regex(@"<a\s+(?:[^>]*?\s+)?href=(['""])(.*?)\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        MatchCollection matches = linkRegex.Matches(htmlCode);
        foreach (Match match in matches)
        {
            string href = match.Groups[2].Value;
            if (!href.StartsWith("http://") && !href.StartsWith("https://")) continue;

            list_url.Add(href);
        }

        return list_url;
    }

    public List<string> get_list_url_on_site(string s_data)
    {
        List<string> list_url = new List<string>();

        string htmlCode = s_data;
        Regex linkRegex = new Regex(@"<a\s+(?:[^>]*?\s+)?href=(['""])(.*?)\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        MatchCollection matches = linkRegex.Matches(htmlCode);
        foreach (Match match in matches)
        {
            string href = match.Groups[2].Value;
            if (!href.StartsWith("http://") && !href.StartsWith("https://"))
            {
                string s_url_on_site = this.s_url + href;
                list_url.Add(s_url_on_site.Replace("//","/"));
            }
        }

        return list_url;
    }

    public void btn_show_list_link_on_site()
    {
        this.show_list_link(this.s_data,true);
    }

    public void btn_show_list_link_off_site()
    {
        this.show_list_link(this.s_data, false);
    }

    private void reset_color_emp()
    {
        this.img_bt_code.color = this.color_nomal;
        this.img_bt_text.color = this.color_nomal;
    }

    public void btn_show_text()
    {
        this.is_view_code = false;
        this.check_model_view();
    }

    public void btn_show_code()
    {
        this.is_view_code = true;
        this.check_model_view();
    }

    public void show_list_link(string s_data,bool is_on_site)
    {
        List<string> list_link;
        this.box_list_link=this.carrot.Create_Box("list_link");

        if (is_on_site)
        {
            this.box_list_link.set_icon(this.data_web.sp_icon_list_link_on_site);
            this.box_list_link.set_title("List links on site");
            list_link = this.get_list_url_on_site(s_data);
        } 
        else
        {
            this.box_list_link.set_icon(this.data_web.sp_icon_list_link_off_site);
            this.box_list_link.set_title("List links off site");
            list_link = this.get_list_url_off_site(s_data);
        }

        for (int i = 0; i < list_link.Count; i++)
        {
            var s_url = list_link[i];
            Carrot.Carrot_Box_Item item_link=this.box_list_link.create_item("link_item_" + i);
            if(is_on_site)
                item_link.set_icon(this.data_web.sp_icon_list_link_on_site);
            else
                item_link.set_icon(this.data_web.sp_icon_list_link_off_site);

            item_link.set_title(list_link[i]);
            item_link.set_tip(list_link[i]);
            item_link.set_act(() => this.open_link(s_url));

            Carrot.Carrot_Box_Btn_Item btn_link=item_link.create_item();
            btn_link.set_icon(this.data_web.sp_icon_web_brower);
            btn_link.set_act(()=>this.open_link(s_url));
            btn_link.set_color(this.carrot.color_highlight);
        }
    }

    public void open_link(string s_link)
    {
        Application.OpenURL(s_link);
    }

    public string get_text(string s_data)
    {
        string s_txt = "";
        string pattern = "<(p|q|h1|h2|h3|h4|h5|h6)[^>]*>(.*?)<\\/(p|q|h1|h2|h3|h4|h5|h6)>";
        MatchCollection matches = Regex.Matches(s_data, pattern);
        foreach (Match match in matches)
        {
            string text = match.Groups[2].Value;
            string cleanedText = Regex.Replace(text, "[^a-zA-Z]", " ");
            s_txt = s_txt + cleanedText+ "\n\n";
        }
        s_txt=s_txt.Replace("     ", " ");
        s_txt=s_txt.Replace("    ", " ");
        s_txt=s_txt.Replace("   ", " ");
        s_txt=s_txt.Replace("  ", " ");
        return s_txt;
    }
}
