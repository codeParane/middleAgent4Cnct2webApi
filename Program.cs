using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
// using System.Net.Http.Headers.Formatting.Extension;


namespace Middle_app
{
    class Program
    {
        static void Main(string[] args)
        {
            //add timer here to iterate 
            MiddleAgent();   
        }

        public static  async Task MiddleAgent()
        {      
            
            string _toTblData = string.Empty;
            string _frmTblData = string.Empty;
            int _totlItemToTbl = 0;
            int _totlItemFromTbl = 0;
                    
            //get data from first_api which is pull the data from db
            using (var pullDataClient = new HttpClient())
            {                
                pullDataClient.BaseAddress = new Uri("http://localhost:5000");
                pullDataClient.DefaultRequestHeaders.Accept.Clear();
                pullDataClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = pullDataClient.GetAsync("api/values/").Result;
                if (response.IsSuccessStatusCode)
                {
                        _frmTblData = response.Content.ReadAsStringAsync().Result;
                }
                //get data from tblTo 
                using (To_Context db = new To_Context())
                {
                    var _tblToDataC = db.tb_data.ToList();
                    _toTblData = JsonConvert.SerializeObject(_tblToDataC, Formatting.None);
                }
            }
            
            //convert list as json objects
            var _lstFromTbl = JsonConvert.DeserializeObject<List<Book>>(_frmTblData);
            var _lstToTbl = JsonConvert.DeserializeObject<List<Book>>(_toTblData);
            //count the records in each list
            _totlItemToTbl = _lstToTbl.Count();
            _totlItemFromTbl = _lstFromTbl.Count();

            //pass the data to second_api which is push the data to db
            using (var pushDataClient = new HttpClient())
            { 
                pushDataClient.BaseAddress = new Uri("http://localhost:4200");
                pushDataClient.DefaultRequestHeaders.Accept.Clear();
                pushDataClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                //check count of both table if not add new records 
                if(_totlItemToTbl != _totlItemFromTbl){
                    for(int i=_totlItemToTbl; i< _totlItemFromTbl;i++){
                        var book = new Book {                 
                            Books = _lstFromTbl[i].Books, Authors=_lstFromTbl[i].Authors
                        };
                        StringContent DataContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                        try
                        {
                            HttpResponseMessage postResponse = pushDataClient.PostAsync("api/values", DataContent).Result;
                        }
                        catch (Exception e)
                        {
                            Console.Write(e);
                        }
                    }
                    Console.WriteLine("worked");
                }
            }
        }
    }
}




               
               
               
               
               
               
               
                //---ipo parung mudiyama irukum yaruuuuuuu loosu you than looosuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu ena
                // villi 


