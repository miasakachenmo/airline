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


        //已经买到的票
        public List<AirLine> GetBuyedTickets(string UserName)
        {
            string Exe = $"select * from airline natural join buyticket where userid=\"{UserName}\"";
            List<AirLine> airLines = new List<AirLine>();
            MySqlDataReader mySqlDataReader = Execute(Exe);

            while (mySqlDataReader.Read())
            {
                AirLine newAirLine = new AirLine();
                newAirLine.arrivetime = mySqlDataReader.GetString("arrivetime");
                newAirLine.date = mySqlDataReader.GetString("date");
                newAirLine.comp = mySqlDataReader.GetString("comp");
                newAirLine.airlinenum = mySqlDataReader.GetString("airlinenum");
                newAirLine.arrivecity = mySqlDataReader.GetString("arrivecity");
                newAirLine.begincity = mySqlDataReader.GetString("begincity");
                newAirLine.begintime = mySqlDataReader.GetString("begintime");
                newAirLine.remainticket = mySqlDataReader.GetInt32("remainticket");
                airLines.Add(newAirLine);
            }
            mySqlDataReader.Close();
            foreach (AirLine newAirLine in airLines)
            {
                newAirLine.status = GetStatus(newAirLine.airlinenum, newAirLine.date);
            }
            return airLines;
        }
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
                        status.iscanceled = true;
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
                newAirLine.remainticket = mySqlDataReader.GetInt32("remainticket");
                airLines.Add(newAirLine);
            }
            mySqlDataReader.Close();
            foreach(AirLine newAirLine in airLines)
            {
                newAirLine.status = GetStatus(newAirLine.airlinenum, newAirLine.date);
            }
            return airLines;
        }

 


        //用户买票，正常买票status为0
        public bool AddTicket(string UserId,string AirlineId,string Date)
        {
            
            string tempo = $"select * from  buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{UserId}\"";
            MySqlDataReader mySqlDataReader = Execute(tempo);
            if (mySqlDataReader.Read())
            {
                mySqlDataReader.Close();
                return false;
            }
            else
            {
                mySqlDataReader.Close();
                string tempo_1 = $"update airline set remainticket=remainticket-1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
                ExecuteNoQuery(tempo_1);
                string tempo_2 = $"INSERT INTO buyticket (airlinenum,date,userid,status) VALUES (\"{AirlineId}\",\"{Date}\" ,\"{UserId}\",'0')";
                ExecuteNoQuery(tempo_2);
                return true;
            }

        }


        //用户退票
        public void DelTicket(string Userid,string AirlineId, string Date)
        {
            string tempo_1 = $"delete from buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{Userid}\"";
            ExecuteNoQuery(tempo_1);
            string tempo_2 = $"update airline set remainticket=remainticket+1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            ExecuteNoQuery(tempo_2);
        }

        //查询信息
        public string[] GetMessage(string Userid)
        {
            return null;
        }
        //插入延误信息
        public void AddMessage(string AirlineId,string Date,string status,string time=null)
        {

        }
        //检查是否有相同航班(检测航班号应该就行)
        public bool HasSameAirline(AirLine airline)
        {
            return false;
        }
        //插入航班(先引用一下上一个函数判断是否是重复航班)
        public void AddAirline(AirLine airline)
        {

        }
        //航班取消
        public void AirlineCanael(string AirlineId,string Date)
        {
            string tempo = $"select * from airlinestatus where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            MySqlDataReader mySqlDataReader = Execute(tempo);
            if (mySqlDataReader.Read())
            {
                string str = $"UPDATE airlinestatus SET status='canceled' where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
                mySqlDataReader.Close();
                ExecuteNoQuery(str);
                return;
            }
            else
            {
                mySqlDataReader.Close();
                string str = $"INSERT INTO airlinestatus (airlinenum,date,status,time) VALUES (\"{AirlineId}\",\"{Date}\" ,'canceled','')";
                ExecuteNoQuery(str);
                return;
            }
        }

        //航班延误
        public void AirlineLate(string AirlineId,string LateTime,string Date)
        {
            string tempo = $"select * from airlinestatus where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            MySqlDataReader mySqlDataReader = Execute(tempo);
            if(mySqlDataReader.Read())
            {
                string str = $"update airlinestatus set time=\"{LateTime}\"where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
                mySqlDataReader.Close();
                ExecuteNoQuery(str);
                return;
            }
            else
            {
                mySqlDataReader.Close();
                string str = $"INSERT INTO airlinestatus (airlinenum,date,status,time) VALUES (\"{AirlineId}\",\"{Date}\" ,'late',\"{LateTime}\")";
                ExecuteNoQuery(str);
                return;
            }
        }

        //用户类型,是否是管理员,是的话返回true (数据库中 管理员表示为0)
        public bool IsAdmin(string Userid)
        {
            string str = $"select usertype from user where userid=\"{Userid}\"";
            MySqlDataReader mySqlDataReader = Execute(str);
            if (mySqlDataReader.Read())
            {
                string type = mySqlDataReader.GetString("usertype");
                mySqlDataReader.Close();
                if (type == "0")
                    return true;
                else
                    return false;
            }
            else
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
                return null;
            }
            string password = reader.GetString("password");
            reader.Close();
            return password;
        }
        public MySqlDataReader Execute(string Command)
        {
            return new MySqlCommand(Command, sqlConnection).ExecuteReader();
        }
        public void ExecuteNoQuery(string Command)
        {
            new MySqlCommand(Command, sqlConnection).ExecuteNonQuery();

        }

        MySqlConnection sqlConnection;


        public  DataBase()
        {
            sqlConnection = new MySqlConnection(ConnectString);
            sqlConnection.Open();
            
        }
    }
}
