using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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
        
        //是否买过票,买过为true，没买过为false
        public bool isbuy(string UserId, string AirlineId, string Date)
        {
            string tempo = $"select * from  buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{UserId}\"";
            MySqlDataReader mySqlDataReader = Execute(tempo);
            if (mySqlDataReader.Read())
            {
                mySqlDataReader.Close();
                return true;
            }
            else
            {
                mySqlDataReader.Close();
                return false;
            }
        }
        


        //用户买票，正常买票status为0
        public void AddTicket(string UserId,string AirlineId,string Date)
        {
            string tempo_1 = $"update airline set remainticket=remainticket-1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            ExecuteNoQuery(tempo_1);
            string tempo_2 = $"INSERT INTO buyticket (airlinenum,date,userid,status) VALUES (\"{AirlineId}\",\"{Date}\" ,\"{UserId}\",'0')";
            ExecuteNoQuery(tempo_2);
        }


        //用户退票
        public void DelTicket(string Userid,string AirlineId, string Date)
        {
            string tempo_1 = $"delete from buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{Userid}\"";
            ExecuteNoQuery(tempo_1);
            string tempo_2 = $"update airline set remainticket=remainticket+1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            ExecuteNoQuery(tempo_2);
        }

        //查询信息乘客所购买航班的延误情况
        public string[] GetMessage(string Userid)
        {   
            return null;
        }
        //检查是否有相同航班(检测航班号应该就行)
        public bool HasSameAirline(AirLine airline)
        {
            string str = $"select * from airline where airlinenum=\"{airline.airlinenum}\"";
            MySqlDataReader mySqlDataReader = Execute(str);
            if (mySqlDataReader.Read())
            {
                mySqlDataReader.Close();
                return true;
            }
            else
            {
                mySqlDataReader.Close();
                return false;
            }
               
        }
        //插入航班(先引用一下上一个函数判断是否是重复航班)
        public void AddAirline(AirLine airline)
        {
            if (HasSameAirline(airline))
                return;
            else
            {

                string str = $"insert into airlinebackup.airline values (\"{airline.comp}\",\"{airline.airlinenum}\",\"{airline.begintime}\",\"{airline.arrivetime}\",\"{airline.remainticket}\",\"{airline.cross}\",\"{airline.begincity}\",\"{airline.arrivecity}\")";
                ExecuteNoQuery(str);
                string str_1 = $"insert into airline.airline select* from airlinebackup.airline inner join airline.date where airlinebackup.airline.airlinenum=\"{airline.airlinenum}\"";
                ExecuteNoQuery(str_1);

            }
        }
        //航班取消
        public void AirlineCanael(string AirlineId,string Date)
        {
            string str = $"replace into airlinestatus values (\"{AirlineId}\",\"{Date}\",'canceled','0')";
            ExecuteNoQuery(str);
        }
        public void AirlineLate(AirLine airLine, TimeSpan LateTime)
        {
            string AirlineId = airLine.airlinenum;
            string Date = airLine.date;
            string str = $"replace INTO airlinestatus VALUES (\"{AirlineId}\",\"{Date}\" ,'late',\"{LateTime}\")";
            ExecuteNoQuery(str);
            //插入延误
            //给用户延误信息

            //先取得受影响的用户ID
            var UseridList = GetUsersBuyedThisTicket(AirlineId, Date);
            string SubAirLine = GetNearAirLine(airLine);
            if (SubAirLine != null)
            {
                string Message = $"您好,您在{Date}的航班{AirlineId}已经延误,您";

            }



        }
        public string GetNearAirLine(AirLine airLine)
        {
            //List<string> temp=new List<string>();
            string Search = $"SELECT * FROM airline Left join airlinestatus on(airline.airlinenum=airlinestatus.airlinenum and airline.date=airlinestatus.date) where airline.begincity =\"{airLine.begincity}\" and airline.arrivecity=\"{airLine.arrivecity}\" and airline.airlinenum!=\"{airLine.airlinenum}\" and airlinestatus.status!=null";
            MySqlDataReader mySqlDataReader = Execute(Search);
            string Temp = null;
            if (mySqlDataReader.Read())
            {
                Temp = (mySqlDataReader.GetString("airlinenum"));
            }
            return Temp;
        }
        //插入延误信息
        public void AddMessage(List<string> userids, string message)
        {
            

        }
        //查到买了这张票的所有用户
        public List<string> GetUsersBuyedThisTicket(string AirlineNum, string Date)
        {
            List<string> temp = new List<string>();
            string SearchStr = $"select userid from buyticket where airlinenum={AirlineNum} and date={Date}";
            MySqlDataReader mySqlDataReader = Execute(SearchStr);
            while (mySqlDataReader.Read())
            {
                temp.Add(mySqlDataReader.GetString("userid"));
            }
            mySqlDataReader.Close();
            return temp;
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
