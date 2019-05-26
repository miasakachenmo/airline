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
        /*public class City
        {
            public string name;
            public string date;
            public List<AirLine> airLines;
            public City(string name, string date) { airLines = Instence.QueryAirlineBeginCity(name, date); this.name = name; this.date = date; }
        }*/

        string ConnectStringLocal = "server=localhost;port=3306;user=airline;password=1234;database=airline;";

        string ConnectString = "server=119.23.219.88;port=3306;user=airline;password=123;database=airline;";


        public void DelAllMessage(string UserId)
        {
            string Exe = $"delete from message where userid=\"{UserId}\"";
            ExecuteNoQuery(Exe);
            return;
        }
        public List<AirLine> GetDayAirLines(string date)
        {
            string Exe = $"select * from  airline as a left join airlinestatus as b on(a.airlinenum=b.airlinenum and a.date=b.date) where a.date=\"{date}\"";
            List<AirLine> airLines = new List<AirLine>();
            mySqlDataReader = Execute(Exe);
            int i = 0;
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
                newAirLine.price = mySqlDataReader.GetInt32("price");
                var time=mySqlDataReader.GetOrdinal("time");
                if(!mySqlDataReader.IsDBNull(time))
                {
                    newAirLine.status = new AirLine.Status();
                    switch (mySqlDataReader.GetString("status"))
                    {
                        case "late":
                            newAirLine.status.islate = true;
                            newAirLine.status.newtime = mySqlDataReader.GetString("time");
                            break;
                        case "canceled":
                            newAirLine.status.iscanceled = true;
                            break;
                    }
                }
                airLines.Add(newAirLine);
                
            }
            mySqlDataReader.Close();
            return airLines;
        }
        //已经买到的票
        public List<AirLine> GetBuyedTickets(string UserName)
        {
            string Exe = $"select * from airline natural join buyticket where userid=\"{UserName}\" and status=\"{0}\"";
            List<AirLine> airLines = new List<AirLine>();
            mySqlDataReader = Execute(Exe);

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
                newAirLine.price = mySqlDataReader.GetInt32("price");
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
            mySqlDataReader = Execute(Exe);
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
        //判断是否有直达
        public bool isHaveStright(string BeginCity, string ArriveCity, string Date)
        {
            string commend = $"select * from airline where begincity=\"{BeginCity}\" and arrivecity=\"{ArriveCity}\" and date=\"{Date}\"";
            MySqlDataReader reader = Execute(commend);
            if(reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }
        //Dijkstra
        /*public List<AirLine> RecommendAirline(string BeginCity,string ArriveCity,string Date)
        {
            string initialCity = $"select * from citys ";
            List<City> Citys=new List<City>();
            MySqlDataReader reader = Execute(initialCity);
            while(reader.Read())
            {
                City city = new City(reader.GetString("city"),Date);
                Citys.Add(city);
            }

            reader.Close();
        }*/
        //查询一个城市所有发出的航线
        public List<AirLine> QueryAirlineBeginCity(string BeginCity, string Date)
        {
            string Exe = $"select * from airline where begincity=\"{BeginCity}\"and date=\"{Date}\"";
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
        //用航班号查询航线
        public List<AirLine> QueryAirlineByAirLineNum(string airlinenum)
        {
            string Exe = $"select * from  airline as a left join airlinestatus as b on(a.airlinenum=b.airlinenum and a.date=b.date) where a.airlinenum=\"{airlinenum}\"";
            List<AirLine> airLines = new List<AirLine>();
            mySqlDataReader = Execute(Exe);

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
                newAirLine.price = mySqlDataReader.GetInt32("price");
                var time = mySqlDataReader.GetOrdinal("time");
                if (!mySqlDataReader.IsDBNull(time))
                {
                    newAirLine.status = new AirLine.Status();
                    switch (mySqlDataReader.GetString("status"))
                    {
                        case "late":
                            newAirLine.status.islate = true;
                            newAirLine.status.newtime = mySqlDataReader.GetString("time");
                            break;
                        case "canceled":
                            newAirLine.status.iscanceled = true;
                            break;
                    }
                }
                airLines.Add(newAirLine);
            }
            mySqlDataReader.Close();
            return airLines;
        }

        //查询航线
        public List<AirLine> QueryAirline(string BeginCity,string ArriveCity,string Date)
        {
            string Exe = $"select * from  airline as a left join airlinestatus as b on(a.airlinenum=b.airlinenum and a.date=b.date) where a.date=\"{Date}\" and begincity=\"{BeginCity}\" and arrivecity=\"{ArriveCity}\"";
            List<AirLine> airLines=new List<AirLine>();
            mySqlDataReader = Execute(Exe);
            
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
                newAirLine.price = mySqlDataReader.GetInt32("price");
                var time = mySqlDataReader.GetOrdinal("time");
                if (!mySqlDataReader.IsDBNull(time))
                {
                    newAirLine.status = new AirLine.Status();
                    switch (mySqlDataReader.GetString("status"))
                    {
                        case "late":
                            newAirLine.status.islate = true;
                            newAirLine.status.newtime = mySqlDataReader.GetString("time");
                            break;
                        case "canceled":
                            newAirLine.status.iscanceled = true;
                            break;
                    }
                }
                airLines.Add(newAirLine);
            }
            mySqlDataReader.Close();
            return airLines;
        }
        
        //是否买过票,买过为true，没买过为false
        public bool isbuy(string UserId, string AirlineId, string Date)
        {
            string tempo = $"select * from  buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{UserId}\"";
            mySqlDataReader = Execute(tempo);
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
        


        //用户买票，正常买票status为0,qi抢票status为1
        public void AddTicket(string UserId,string AirlineId,string Date,string status="0")
        {
            string tempo_1;
            if (status.Equals("0"))
            {
                tempo_1 = $"update airline set remainticket=remainticket-1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
                ExecuteNoQuery(tempo_1);
            }
            string tempo_2 = $"INSERT INTO buyticket (airlinenum,date,userid,status) VALUES (\"{AirlineId}\",\"{Date}\" ,\"{UserId}\",'{status}')";
            ExecuteNoQuery(tempo_2);    
        }

        //用户抢票
        public void TryTicket(string UserId,string AirlineId,string Date)
        {
            string str = $"INSERT INTO buyticket (airlinenum,date,userid,status) VALUES (\"{AirlineId}\",\"{Date}\" ,\"{UserId}\",'1')";
            ExecuteNoQuery(str);
        }

        //放票
        public void GiveTicket(string AirlineId, string Date)
        {
            string str = $"select * from buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and status=1 order by num limit 1";
            mySqlDataReader = Execute(str);
            if(mySqlDataReader.Read())
            {
                string id = mySqlDataReader.GetString("userid");
                mySqlDataReader.Close();
                string tempo_1= $"update buyticket set status='0' where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{id}\"";
                ExecuteNoQuery(tempo_1);
                string tempo_2 = $"update airline set remainticket=remainticket-1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
                ExecuteNoQuery(tempo_2);
                string tempo_messsage = $"航班号：{AirlineId} 时间：{Date}抢票成功";
                AddMessage(new List<string>() {id }, tempo_messsage);
            }
            else
            {
                mySqlDataReader.Close();
                return;
            }
        }
       

        //用户退票
        public void DelTicket(string Userid,string AirlineId, string Date)
        {
            string tempo_1 = $"delete from buyticket where airlinenum=\"{AirlineId}\"and date=\"{Date}\"and userid=\"{Userid}\"";
            ExecuteNoQuery(tempo_1);
            string tempo_2 = $"update airline set remainticket=remainticket+1 where airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            ExecuteNoQuery(tempo_2);
            string commend = $"select * from airline where remainticket=1 and airlinenum=\"{AirlineId}\"and date=\"{Date}\"";
            mySqlDataReader = Execute(commend);
            if(mySqlDataReader.Read())
            {
                mySqlDataReader.Close();
                GiveTicket(AirlineId, Date);
            }
            else
            {
                mySqlDataReader.Close();
                return;
            }
        }

        //查询信息乘客所购买航班的延误情况
        public List<string> GetMessage(string Userid)
        {
            List<string> Temp = new List<string>();
            string sqlstr = $"select * from message where userid=\"{Userid}\"";
            mySqlDataReader = Execute(sqlstr);

       //   for(int i=0;i<5;i++)
       //   {
       //       bool a=mySqlDataReader.Read();
       //       
       //       Temp.Add(mySqlDataReader.GetString("message"));
       //   }
            while(mySqlDataReader.Read())
            {
                Temp.Add(mySqlDataReader.GetString("message"));
            }
            mySqlDataReader.Close();
            return Temp;
        }
        //检查是否有相同航班(检测航班号应该就行)
        public bool HasSameAirline(AirLine airline)
        {
            string str = $"select * from airline where airlinenum=\"{airline.airlinenum}\"";
            mySqlDataReader = Execute(str);
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
        public bool AddAirline(AirLine airline)
        {
            if (HasSameAirline(airline))
                return false;
            else
            {

                string str = $"insert into airlinebackup.airline values (\"{airline.comp}\",\"{airline.airlinenum}\",\"{airline.begintime}\",\"{airline.arrivetime}\",\"{airline.remainticket}\",\"{airline.cross}\",\"{airline.begincity}\",\"{airline.arrivecity}\",\"{airline.price}\")";
                ExecuteNoQuery(str);
                string str_1 = $"insert into airline.airline select* from airlinebackup.airline inner join airline.date where airlinebackup.airline.airlinenum=\"{airline.airlinenum}\"";
                ExecuteNoQuery(str_1);
                return true;
            }
        }
        //航班取消
        public void AirlineCanael(AirLine airLine)
        {
            string AirlineId = airLine.airlinenum;
            string Date = airLine.date;
            string str = $"replace into airlinestatus values (\"{AirlineId}\",\"{Date}\",'canceled','0')";

            ExecuteNoQuery(str);
            //先取得受影响的用户ID
            var UseridList = GetUsersBuyedThisTicket(AirlineId, Date);
            string SubAirLine = GetNearAirLine(airLine);
            string Message;
            if (SubAirLine != null)
            {
                Message = $"您好,您在{Date}的航班{AirlineId}已经取消,您可以选择搭乘最近的航班{SubAirLine}";
            }
            else
            {
                Message = $"您好,您在{Date}的航班{AirlineId}已经取消,请谅解";
            }
            AddMessage(UseridList, Message);
        }

        //修改内存里的航班->修改数据库里的航班状态->插入消息
        public void AirlineLate(AirLine airLine, string LatetimeMin)
        {
            airLine.status.newtime = LatetimeMin;
            airLine.status.islate = true;

            string AirlineId = airLine.airlinenum;
            string Date = airLine.date;
            string str = $"replace INTO airlinestatus VALUES (\"{AirlineId}\",\"{Date}\" ,'late',\"{LatetimeMin}\")";
            ExecuteNoQuery(str);
            //插入延误
            //给用户延误信息

            //先取得受影响的用户ID
            var UseridList = GetUsersBuyedThisTicket(AirlineId, Date);
            string SubAirLine = GetNearAirLine(airLine);
            string Message;
            if (SubAirLine != null)
            {
                Message = $"您好,您在{Date}的航班{AirlineId}已经延误{LatetimeMin}分钟,您可以选择搭乘最近的航班{SubAirLine}";
            }
            else
            {
                Message = $"您好,您在{Date}的航班{AirlineId}已经延误{LatetimeMin}分钟,请谅解";
            }
            AddMessage(UseridList, Message);


        }
        public string GetNearAirLine(AirLine airLine)
        {
            //List<string> temp=new List<string>();
            string Search = $"SELECT * FROM airline Left join airlinestatus on(airline.airlinenum=airlinestatus.airlinenum and airline.date=airlinestatus.date) where airline.begincity =\"{airLine.begincity}\" and airline.arrivecity=\"{airLine.arrivecity}\" and airline.date=\"{airLine.date}\" and airline.airlinenum!=\"{airLine.airlinenum}\" and airlinestatus.status is NULL";
            mySqlDataReader = Execute(Search);
            string Temp = null;
            if (mySqlDataReader.Read())
            {
                Temp = (mySqlDataReader.GetString("airlinenum"));
            }
            mySqlDataReader.Close();
            return Temp;
        }
        //插入延误信息
        public void AddMessage(List<string> userids, string message)
        {
            string sqlstr = "";
            
            foreach (string userid in userids)
            {
                sqlstr += $"insert into message(userid,message) value(\"{userid}\",\"{message}\");";
            }
            if(sqlstr!="")
                ExecuteNoQuery(sqlstr);
        }
        //查到买了这张票的所有用户
        public List<string> GetUsersBuyedThisTicket(string AirlineNum, string Date)
        {
            List<string> temp = new List<string>();
            string SearchStr = $"select userid from buyticket where airlinenum=\"{AirlineNum}\" and date=\"{Date}\"";
            mySqlDataReader = Execute(SearchStr);
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
            mySqlDataReader = Execute(str);
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
            mySqlDataReader = Execute(str);
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

        MySqlDataReader mySqlDataReader;
        public  DataBase()
        {
            sqlConnection = new MySqlConnection(ConnectString);
            sqlConnection.Open();
            
        }
    }
}
