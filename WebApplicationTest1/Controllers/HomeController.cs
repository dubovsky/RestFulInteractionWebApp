using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest1.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace WebApplicationTest1.Controllers
{
    public class HomeController : Controller
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Mobile>));

        //item to delete
        private static string itemName=null;

        //you must change port number, because it may differ from your own
        private string URL = "http://localhost:57447/api/RestAPI";

        //Home/Get
        public async Task<IActionResult> Index()
        {
            List<Mobile> result = new List<Mobile>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(URL);

                response.EnsureSuccessStatusCode();

                byte[] arr = await response.Content.ReadAsByteArrayAsync();

                result = JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(arr), typeof(List<Mobile>)) as List<Mobile>;
            } 
            return View(result);
        }

        //Home/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name, IFormFile imageUpload)
        {
            // the path to the file in temp location
            string filePath;

            if (String.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("Name", "Type name please!");

                return View();
            }

            if (ModelState.IsValid)
            {
                if (imageUpload != null)
                {
                    var count = imageUpload.Length;

                    string imagePath = imageUpload.FileName;

                    filePath = Path.GetFileName(imagePath);

                    string SourceURL = Path.Combine("wwwroot/images/", filePath);

                    Mobile mobile = new Mobile(Name, SourceURL);

                    //send data to rest api and it will rewrite json file data
                    using (HttpClient client = new HttpClient())
                    { 
                        string con = JsonConvert.SerializeObject(mobile);

                        var content = new StringContent(con, Encoding.Default, "application/json");

                        var responseMessage = await client.PostAsync(URL, content);
                    }                   
                    return RedirectToAction("Index");
                }
                return NotFound();
                
            }
            else return View();       
        }

        //Home/Update
        [HttpGet]
        public async Task<IActionResult> Update(string name)
        {
            if (name != null)
            {
                List<Mobile> result = new List<Mobile>();

                //get current list of mobiles
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(URL);

                    response.EnsureSuccessStatusCode();

                    byte[] arr = await response.Content.ReadAsByteArrayAsync();

                    result = JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(arr), typeof(List<Mobile>)) as List<Mobile>;
                }              

                var item = result.Where(x => x.Name == name).FirstOrDefault();
                if (item!=null)
                {
                    itemName = name;
                    return View(item);
                }

                return NotFound();
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string Name, IFormFile imageUpload)
        {
            // the path to the file in temp location
            string filePath;

            if (imageUpload != null&&Name!=null)
            {
                if (itemName != null)
                {
                    //delete current item first, then add item to json file
                    await this.Delete(itemName);
                }
                else return BadRequest();
               
                //process update operation
                var count = imageUpload.Length;

                string imagePath = imageUpload.FileName;

                filePath = Path.GetFileName(imagePath);

                string SourceURL = Path.Combine("wwwroot/images/", filePath);

                Mobile mobile = new Mobile(Name, SourceURL);

                //send data to rest api and it will rewrite json file data
                using (HttpClient client = new HttpClient())
                {
                    string con = JsonConvert.SerializeObject(mobile);

                    var content = new StringContent(con, Encoding.Default, "application/json");

                    var responseMessage = await client.PutAsync(URL, content);
                }
                return RedirectToAction("Index");
            }
            return NotFound();

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            if (name != null)
            {
                List<Mobile> result = new List<Mobile>();

                //get current list of mobiles
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(URL);

                    response.EnsureSuccessStatusCode();

                    byte[] arr = await response.Content.ReadAsByteArrayAsync();

                    result = JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(arr), typeof(List<Mobile>)) as List<Mobile>;
                }

                var item = result.Where(x => x.Name == name).FirstOrDefault();

                //send request to rest api to delete item
                if (item != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string con = JsonConvert.SerializeObject(item);
                    
                        var responseMessage = await client.SendAsync(new HttpRequestMessage()
                        {
                            Method = HttpMethod.Delete,
                            RequestUri=new Uri(URL+"/"+item.Name)
                        });
                    }
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View("Index");
        }

    }
}
