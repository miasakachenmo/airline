﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PlaneUWP
{
    public class DataBase
    {
        private static DataBase instence;
        public static DataBase Instence
        {
            get
            {
                if (instence == null)
                    instence = new DataBase();
                return instence;
            }
            set
            {
                instence = value;
            }
        }

        string ConnectStringLocal = "server=localhost;port=3306;user=airline;password=1234;database=airline;";

        string ConnectString = "server=119.23.219.88;port=3306;user=airline;password=123;database=airline;";

        public AirLine.Status GetStatus(string AirLineNum,string Date)
        {
            string Exe = $"select * from airlinestatus where airlinenum=\"{AirLineNum}\" and date=\"{Date}\"";
            AirLine.Status status = new AirLine.Status();
            MySqlDataReader mySqlDataReader = Execute(Exe);
            if(mySqlDataReader.Read())
            {
                switch (mySqlDataReader.GetString("status"))
                {
                    case "late":
                        status.islate = true;
                        status.newtime = mySqlDataReader.GetString("time");
                        break;
                    case "canceled":
                        status.iscanceled = false;
                        break;
                }
            }
            mySqlDataReader.Close();
            return status;
            
        }
        //查询航线
        public List<AirLine> QueryAirline(string BeginCity,string ArriveCity,string Date)
        {
            string Exe = $"select * from airline where begincity=\"{BeginCity}\" and arrivecity=\"{ArriveCity}\" and date=\"{Date}\"";
            List<AirLine> airLines=new List<AirLine>();
            MySqlDataReader mySqlDataReader = Execute(Exe);
            
            while (mySqlDataReader.Read())
            {
                AirLine newAirLine = new AirLine();
                newAirLine.arrivetime = mySqlDataReader.GetString("arrivetime");
                newAirLine.date = mySqlDataReader.GetString("date");
                newAirLine.comp= mySqlDataReader.GetString("comp");
                newAirLine.airlinenum = mySqlDataReader.GetString("airlinenum");
                newAirLine.arrivecity = mySqlDataReader.GetString("arrivecity");
                newAirLine.begincity = mySqlDataReader.GetString("begincity");
                newAirLine.begintime = mySqlDataReader.GetString("begintime");
                newAirLine.remainticket = mySqlDataReader.GetString("remainticket");
                airLines.Add(newAirLine);
            }
            mySqlDataReader.Close();
            foreach(AirLine newAirLine in airLines)
            {
                newAirLine.status = GetStatus(newAirLine.airlinenum, newAirLine.date);
            }
            return airLines;
        }

 


        //用户买票
        public void AddTicket(string UserId,string AirlineId,string Date)
        {
        }


        //用户退票
        public void DelTicket(string Userid,string AirlineId, string Date)
        {

        }

        //查询信息
        public string[] GetMessage(string Userid)
        {
            return null;
        }

        //航班取消
        public void AirlineCanael(string AirlineId,string Date)
        {

        }

        //航班延误
        public void AirlineLate(string AirlineId,string LateTime,string Date)
        {

        }

        //用户类型,是否是管理员,是的话返回true
        public bool IsAdmin(string Userid)
        {
            string str = $"select usertype from user where userid=\"{Userid}\"";
            MySqlDataReader mySqlDataReader = Execute(str);
            mySqlDataReader.Read();
            string type=mySqlDataReader.GetString("usertype");
            mySqlDataReader.Close();
            if (type == "1")
                return true;
            return false;
        }
        
        //判断密码
        public bool isPassword(string Userid,string Password)
        {
            string str = $"select password from user where userid=\"{Userid}\"";
            MySqlDataReader mySqlDataReader = Execute(str);
            if (!mySqlDataReader.Read())
                return false;
            else
            {
                string tempo = mySqlDataReader.GetString("password");
                if (tempo == Password)
                    return true;
                else
                    return false;
            }
        }

        //用户密码
        public string GetPassWord(String Userid)
        {
            string str = $"select password from user where userid=\"{Userid}\"";
            MySqlDataReader reader = Execute(str);
            if (!reader.Read())
            {
                reader.Close();
                return "";
            }
            string password = reader.GetString("password");
            reader.Close();
            return password;
        }
        public MySqlDataReader Execute(string Command)
        {
            return new MySqlCommand(Command, sqlConnection).ExecuteReader();
        }

        MySqlConnection sqlConnection;


        public  DataBase()
        {

            sqlConnection = new MySqlConnection(ConnectStringLocal);
            sqlConnection.Open();
            
        }
    }
}
