using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADCP
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }
 
     //   public bool btnOK = false;
        public StationInfo stationInf;
        private void button_ok_Click(object sender, EventArgs e)
        {
            stationInf.station = textBox_station.Text.ToString(); //LPJ 2012-7-10
            stationInf.weather = textBox_weather.Text.ToString();
            stationInf.wind = textBox_wind.Text.ToString();
            stationInf.surveyboat = textBox_surveyboat.Text.ToString();
            stationInf.computer = textBox_computer.Text.ToString();
            stationInf.kinemometer = textBox_kinemometer.Text.ToString();
            stationInf.hardware = textBox_hardware.Text.ToString();
            stationInf.software = textBox_software.Text.ToString();
            stationInf.GPS = textBox_GPS.Text.ToString();
            stationInf.Compass = textBox_compass.Text.ToString();
            stationInf.acousticSounder = textBox_acousticSounder.Text.ToString();
            stationInf.remark = textBox_remark.Text.ToString();
            stationInf.operate = textBox_operator.Text.ToString();
            stationInf.check = textBox_check.Text.ToString();
            stationInf.examine = textBox_examine.Text.ToString();

            stationInf.startWL = textBox_StartWL.Text;
            stationInf.endWL = textBox_EndWL.Text;
            stationInf.MeanWL = textBox_MeanWL.Text;

        //    btnOK = true;
            Close();
          
        }

        public void ShowEdit(StationInfo sInf) //LPJ 2012-7-10 add
        {
            textBox_station.Text = sInf.station;
            textBox_weather.Text = sInf.weather;
            textBox_wind.Text = sInf.wind;
            textBox_surveyboat.Text = sInf.surveyboat;
            textBox_computer.Text = sInf.computer;
            textBox_kinemometer.Text = sInf.kinemometer;
            textBox_hardware.Text = sInf.hardware;
            textBox_software.Text = sInf.software;
            textBox_GPS.Text = sInf.GPS;
            textBox_compass.Text = sInf.Compass;
            textBox_acousticSounder.Text = sInf.acousticSounder;
            textBox_remark.Text = sInf.remark;
            textBox_operator.Text = sInf.operate;
            textBox_check.Text = sInf.check;
            textBox_examine.Text = sInf.examine;

            textBox_StartWL.Text = sInf.startWL;
            textBox_EndWL.Text = sInf.endWL;
            textBox_MeanWL.Text = sInf.MeanWL;
        }

        public struct StationInfo
        {
            public string station;     //测站
            public string weather;     //天气
            public string wind;        //风力风向
            public string surveyboat;  //测船
            public string computer;    //计算机名
            public string kinemometer; //流速仪
            public string hardware;    //硬件版本
            public string software;    //软件版本
            public string GPS;         //GPS型号
            public string Compass;     //罗经型号
            public string acousticSounder;  //测深仪型号
            public string remark;      //备注
            public string operate;
            public string check;
            public string examine;

            public string startWL;     //开始水位
            public string endWL;       //结束水位
            public string MeanWL;      //平均水位
        };

           
    }
}
