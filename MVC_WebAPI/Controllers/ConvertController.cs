using MVC_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MVC_WebAPI.Controllers
{
    public class ConvertController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public String Post([FromBody] TextInput textInput)
        {
            string input = textInput.UserText;
            List<String> totalList = ToLong(input.ToLower());
            string output = "";
            foreach (var item in totalList)
            {
                output = output + item + " ";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        public static Dictionary<string, long> numberTable = new Dictionary<string, long>
        {
            { "sıfır",0},{"bir",1},{"iki",2},{"üç",3},{"dört",4},
            {"beş",5},{"altı",6},{"yedi",7},{"sekiz",8},{"dokuz",9},
            {"on",10},{"onbir",11},{"oniki",12},{"onüç",13},
            {"ondört",14},{"onbeş",15},{"onaltı",16},
            {"onyedi",17},{"onsekiz",18},{"ondokuz",19},{"yirmi",20},
            {"otuz",30},{"kırk",40},{"elli",50},{"altmış",60},
            {"yetmiş",70},{"seksen",80},{"doksan",90},{"yüz",100},
            {"bin",1000},{"yüz bin",100000},{"milyon",1000000},{"milyar",1000000000},
            {"trilyon",1000000000000},{"katrilyon",1000000000000000},
            {"kentrilyon",1000000000000000000}
        };

        public static List<String> ToLong(string numberString)
        {
            string[] split = numberString.Split(' ');
            int index = 0;

            List<String> outputList = new List<string>();

            var numbers = Regex.Matches(numberString, @"\w+").Cast<Match>()
                 .Select(m => m.Value.ToLowerInvariant())
                 .Where(v => numberTable.ContainsKey(v))
                 .Select(v => numberTable[v]);

            var text = Regex.Matches(numberString, @"\w+").Cast<Match>()
                .Select(m => m.Value.ToLowerInvariant())
                .Where(v => !numberTable.ContainsKey(v));

            long acc = 0, total = 0L;

            foreach (var n in numbers)
            {
                if (n >= 1000)
                {
                    if (index == 0)
                        acc = 1;
                    total += (acc * n);
                    acc = 0;
                }
                else if (n >= 100)
                {
                    if (index == 0)
                        acc = 1;
                    acc *= n;
                }
                else acc += n;
                index++;
            }
            bool flag = false;

            for (int i = 0; i < split.Length; i++)
            {
                if (!(numberTable.Keys).Contains(split[i]))
                {
                    outputList.Add(split[i]);
                }
                else
                {
                    if (!flag)
                    {
                        long toplam = total + acc;
                        outputList.Add(toplam.ToString());
                        flag = true;
                    }
                }
            }
            return outputList;
        }
    }
}