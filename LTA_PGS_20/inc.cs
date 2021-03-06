﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace LTA_PGS_20
{
    static class inc
    {
        public static DB_Control db_control = null;
        public static TCPClient tcp_client = null;
        public static RemoteFile remote_disk = null;

        public static int currentuser=0;
        public static string currentpwd = "";
        public static string username = "";
        public static int alarm_check_timer = 0;
        public static string tabpagename = "";
        public static List<String> pagelist = null;

        public static string split_sign = "&";
        public static System.Object[] VMS_Diming_mode = new System.Object[5] { "None", "Day", "Night", "Day/Night", "SensorAuto" };
        public static System.Object[] VMS_Flash_mode = new System.Object[3] { "Toggle", "Scroll", "Flash" };
        public static System.Object[] VMS_Lots_color = new System.Object[4] { "Green", "Amber", "Red", "Variable" };
        public static System.Object[] VMS_Color_mode = new System.Object[2] { "Lots", "Bar" };
        public static System.Object[] VMS_Name_color = new System.Object[3] { "Green", "Amber", "Red" };
        
        public static System.Object[] VMS;

        public static System.Object[] Pre_Message;
        public static System.Object[] User_Group = new System.Object[3] { "Operator", "Supervisor", "Administrator" };
        public static System.Object[] WeekDays = new System.Object[7] { "Monday", "Tuesday", "Wednesday","Thursday","Friday","Saturday","Sunday" };
        public static System.Object[] WeekDays_Holiday = new System.Object[8] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday","Holiday" };
        public static System.Object[] Dimming_mode = new System.Object[2] { "Manual", "Auto" };

        public static System.Object[] VMS_color_code = new System.Object[3] { "^CG", "^CA", "^CR" };

        public static System.Object[] DateGroup = new System.Object[5] { "Date Group 1", "Date Group 2", "Date Group 3", "Date Group 4", "Date Group 5" };

        public static System.Object[] Area = null;//new System.Object[3] { "Marina", "Orchard", "Harbfront" };//1 - Marina  2- Orchard 3 - Harbfront
        //public static int[] Area_id = null;//new int[3] { 1, 2, 3 };//1 - Marina  2- Orchard 3 - Harbfront
        public static string[] error_msg = new string[12] { "CCS SERVER DOWN / DISK FULL"
            , "VMS DISCONNECT"
            , "VMS POWER SUPPLY FAIL"
            , "VMS ERROR"
            , "VMS SCANBOARD ERROR"
            , "VMS SENSOR ERROR"
            , "UPDATE FAIL"
            , "VMS PIXEL FAILURE"
            , "CONFIG ERROR"
            , "CMS DISCONNECT"
            , "LDC DISCONNECT"
            , "EA DISCONNECT"};

        public static string[] Month_Display = new string[13] { "","Jan"
            , "Feb"
            , "Mar"
            , "Apr"
            , "May"
            , "Jun"
            , "Jul"
            , "Aug"
            , "Sep"
            , "Oct"
            , "Nov"
            , "Dec"};

        public enum LOGMSGCODE
        {
            ADU01=1,
            ADI01,
            ADD01,
            ALU01,
            ARU01,
            ARI01,
            ARD01,
            CPU01,
            CPI01,
            CPD01,
            VMU01,
            VMI01,
            VMD01,
            HDU01,
            HDI01,
            HDD01,
            MNU01,
            PMU01,
            PMI01,
            PMD01,
            SCU01,
            SCI01,
            SCD01,
            STU01,
            STI01,
            STD01,
            USU01,
            USI01,
            USD01,
            RSC01
        }

        public static Color Amber = Color.FromArgb(255, 122, 3);
        public static Color[] VMS_Color = new Color[3] { Color.Green, Amber, Color.Red };
        public static System.Object[] carparkobject;

        public static int USER_TIME_OUT = 3600;
        public struct pre_msg
        {
            public int id;
            public int gdm;
            public int color;
            //public int map_id;
            //public Point map_point;
            public string name;
            public string line1;
            public string line2;
            public string line3;
            public string line4;
            public string line5;
            public string line6;
        }
        public static pre_msg[] Pre_Msg = null;

        public struct carpark
        {
            public int id;
            public int limit;
            public int map_id; 
            public Point map_point;            
            public string name;
            public string name2;
            public string ip;

            public int x_out;
            public int y_out;

            public string f_time;
            public string t_time;

            public int area;
            public int status;//update by alarm timer
            //public int ack;//update by alarm timer
            public int lots;
            public string latitude;
            public string longtitude;
        }
        public static carpark[] Carpark = null;

        public struct vms_detail
        {
            public int name_color;
            public int lots_color;
            public int mode;
            public int green;
            public int red;
            public int index;
            public bool update;
        }
        public static vms_detail VMS_Detail;

        public struct vms_str
        {
            public int _id;
            public string name;
            public string lcoation;
            public int dim;
            public int timer;
            public int flash;
            public int x;
            public int y;
            public int area;
            public int status;//update by alarm timer
            //public int ack;//update by alarm timer
        }
        public static vms_str[] _vms = null;


        public struct users_str
        {
            public int id;
            public string name;
            public string pwd;
            public int level;
            public int times;
            public DateTime lasttime;
        }
        public static users_str[] TB_user = null;

        public struct adhoc_group
        {
            public int groupid;
            public int timer;
            public int mode;
            public int color;
            public List<Int64> id;
            public string rows;
            public List<string> gdm_list;
            public DateTime starttime;
            public DateTime endtime;
            public DateTime addtime;
            public Int64 useendtime;
            public string name;
        }
        public static adhoc_group[] Adhoc_Group;
        public static int Group_id = 0;

        public struct timezone_group_msg_str
        {
            public Int64 schedule_id;       //id in timezone_group_str
            public string timezones;
            public string msg;
            public Int64 mode;
            public Int64 timer;            
            public Int64 id;
            public Byte pattern;
        }

        //public struct timezone_group_str
        //{            
        //    public Int64 id;
        //    public Int64 schedule_id;       //id in schedule
        //    public List<timezone_group_msg_str> tz_group_msg;
        //}
        public struct schedule
        {
            public string name;
            //public Int64 timer;
            //public Int64 mode;
            public Int64 useendtime;
            //public Int64 delete;
            //public Int64 user;
            public DateTime starttime;
            public DateTime endtime;
            public Int32 id;
            //public string rows;
            public string datezone;
            public List<string> gdm_list;
            public List<timezone_group_msg_str> timezone_group;
        }
        public static schedule[] tb_schedule;


        public struct area
        {
            public int fd_id;
            public int areaid;
            public int x;
            public int y;
            public string name;
            public Size map_size;
            public string map_name;
            public int show;
        }
        public static area[] tb_area;

        public struct logmsg
        {
            public string code;
            public string msg;
            public int id;
        }
        public static logmsg[] tb_logmessage;


        public static string images_path = "";

        public static int auto_log_off = 0;
        public static bool log_off;

        public static bool server_status_ok;

        public static bool databaseerror = true;
    }
}