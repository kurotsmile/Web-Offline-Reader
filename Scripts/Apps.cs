using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Apps : MonoBehaviour
{
    [Header("Obj Main")]
    public Carrot.Carrot carrot;
    public Manage_Data_Web data_web;
    public View_Data data_view;
    public AudioSource audio_bk;

    [Header("Ui")]
    public InputField InputField_websiteUrl;
    public Text txt_scores_ranks;

    private int scores_ranks = 0;
    private Carrot.Carrot_Window_Loading loading_create;
    private Carrot.Carrot_Box box_setting;
 
    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.data_web.on_load();
        this.data_view.on_load();

        this.carrot.game.load_bk_music(this.audio_bk);
        this.scores_ranks = PlayerPrefs.GetInt("scores_ranks",0);
        this.update_ui_emp();
    }

    private void check_exit_app()
    {
        if (this.data_view.panel_view_data.activeInHierarchy)
        {
            this.data_view.btn_close();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_create_web()
    {
        this.add_ranks();
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.loading_create=this.carrot.show_loading(this.act_create_web());
    }

    IEnumerator act_create_web()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(this.InputField_websiteUrl.text))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                this.carrot.Show_msg("Error", www.error, Carrot.Msg_Icon.Error);
                this.loading_create.close();
                this.carrot.play_vibrate();
            }
            else
            {
               string s_data = www.downloadHandler.text;
               string s_url = this.InputField_websiteUrl.text;
               this.data_view.show(s_url,s_data);
               this.data_web.add_data(s_url, s_data);
                this.loading_create.close();
            }
        }
    }

    public void btn_user()
    {
        this.carrot.user.show_login();
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public void btn_app_other()
    {
        this.carrot.show_list_carrot_app();
    }

    public void btn_list_data_web()
    {
        this.carrot.play_sound_click();
        this.data_web.show_list_data_web();
    }

    public void btn_setting()
    {
        this.box_setting=this.carrot.Create_Setting();

        Carrot.Carrot_Box_Item btn_list_web=box_setting.create_item_of_index("list_web",0);
        btn_list_web.set_icon_white(this.data_web.sp_icon_list_web);
        btn_list_web.set_title("List of created websites");
        btn_list_web.set_tip("You can use saved web pages while offline");
        btn_list_web.set_act(this.act_show_list_web_data);
        btn_list_web.set_type(Carrot.Box_Item_Type.box_nomal);
        btn_list_web.check_type();
    }

    private void act_show_list_web_data()
    {
        if (this.box_setting != null) this.box_setting.close();
        this.btn_list_data_web();
    }

    public void btn_ranks()
    {
        this.carrot.game.Show_List_Top_player();
    }

    private void update_ui_emp()
    {
        this.txt_scores_ranks.text = this.scores_ranks.ToString();
    }

    private void add_ranks()
    {
        this.scores_ranks++;
        PlayerPrefs.SetInt("scores_ranks", this.scores_ranks);
        this.update_ui_emp();

        if (Random.Range(0, 2) == 1)
        {
            this.carrot.game.update_scores_player(this.scores_ranks);
        }
    }
}