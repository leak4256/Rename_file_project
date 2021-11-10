﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SimplyTravelDAL;
using Newtonsoft.Json;
using Models;
namespace SimpltyTravelBLL
{
    public class Algurithm
    {
        #region data
        DBConnection dbCon;
        Random random = new Random();
        //lists cf data
        List<Sites> listOfSites;
        #endregion
        public Algurithm()
        {
            dbCon = new DBConnection();
        }
        //dictionary of the time spend
        Dictionary<bool, int> misHours = new Dictionary<bool, int>()
        {
            { true, 7 },
            { false, 14 }
        };
        //arr of num of the site each type in a trip 
        double[] numOfSitesOfType;
        #region function
        //יצירת הרכב טיולים ללקוח
        public Dictionary<int, List<Sites>> CreateTravels(int MinAge, int MaxAge, bool Car_bus, 
            bool halfDay_allDay, int codeSubRegion,string myAddress)
        {
            listOfSites = dbCon.GetDbSet<Sites>().Where(i => i.minAge <= MinAge && i.maxAge >= MaxAge 
            && i.car_bus == Car_bus 
            && i.codeSub_Region == codeSubRegion&& i.statusSite=="active").ToList();
            //dictionary of 5 options to trips
            Dictionary<int, List<Sites>> dictOfTrips = new Dictionary<int, List<Sites>>();
            //list of coffee shops 
            List<Sites> coffeeShops = listOfSites.Where(i => i.codeSiteKind == 2).ToList();
            //list of restaurants
            List<Sites> restaurants = listOfSites.Where(i => i.codeSiteKind == 3).ToList();
            //list of tombs-קברים
            List<Sites> tombs = listOfSites.Where(i => i.codeSiteKind == 1).ToList();
            //list of dry trails 
            List<Sites> dryTrails = listOfSites.Where(i => i.codeSiteKind == 7 ).ToList();
            //list of wet trails 
            List<Sites> wetTrails = listOfSites.Where(i => i.codeSiteKind == 6 ).ToList();
            //list of dry attractions 
            List<Sites> dryAttractions= listOfSites.Where(i => i.codeSiteKind == 5 ).ToList();
            //list of wet attractions 
            List<Sites> wetAttractions = listOfSites.Where(i => i.codeSiteKind == 4).ToList();
            //list of nature reserves
            List<Sites> natureReserves = listOfSites.Where(i => i.codeSiteKind == 8).ToList();
            //list of museuns
            List<Sites> museuns = listOfSites.Where(i => i.codeSiteKind == 9).ToList();
            //mat to calculation number of sites from all of kind
            double[,] matPercents = new double[5, 9] { { 0, 15, 1, 22, 22, 0, 0, 40, 0 }, 
                { 10, 10, 10, 15, 15, 0, 0, 20, 20 }, { 0, 10, 17, 0, 0, 0, 0, 28, 45 },
                { 0, 15, 0, 17.5, 17.5, 25, 25, 0, 0 }, { 5, 5, 18, 9, 9, 9, 9, 18, 18 } };
            //arr of the names trips kind     
            string[] arrNamesKinds = new string[5] { "טבע", "רגוע", "הסטוריה", "אתגרי", "קלאסי" };
            //arr of the list trips kind     
            List<Sites>[] arrListKinds = new List<Sites>[9] {coffeeShops,restaurants,tombs,dryTrails,
                wetTrails,dryAttractions,wetAttractions,natureReserves,museuns };
            //number of sites in a trip
            //get the average time to a site
            SiteBL s = new SiteBL();
            int avg = s.GetAvgTime();
            //local variable
            double misLeft;
            //mis sites in trip
            int numOfSiteInTrip = misHours[halfDay_allDay] / avg;
            for (int i = 0; i < 5; i++)
            {
                //number of sites per type 
                numOfSitesOfType = new double[9];
                for (int j = 0; j < 9; j++)
                {
                    numOfSitesOfType[j] = Math.Floor(numOfSiteInTrip * (matPercents[i, j] * 0.01));
                }
                //Refresh
                if (i > 0)
                {
                    coffeeShops = listOfSites.Where(c => c.codeSiteKind == 1).ToList();
                    restaurants = listOfSites.Where(c => c.codeSiteKind == 2).ToList();
                    tombs = listOfSites.Where(c => c.codeSiteKind == 3).ToList();
                    natureReserves = listOfSites.Where(c => c.codeSiteKind == 8).ToList();
                    museuns = listOfSites.Where(c => c.codeSiteKind == 9).ToList();
                    if (i == 1)
                    {
                        dryTrails = listOfSites.Where(c => c.codeSiteKind == 4
                        && c.extraLevel==1).ToList();
                        wetTrails = listOfSites.Where(c => c.codeSiteKind == 5 
                        && c.extraLevel == 1).ToList();
                        dryAttractions = listOfSites.Where(c => c.codeSiteKind == 6 
                        && c.extraLevel == 1).ToList();
                        wetAttractions = listOfSites.Where(c => c.codeSiteKind == 7 
                        && c.extraLevel == 1).ToList();
                    }
                    if (i == 3)
                    {
                        dryTrails = listOfSites.Where(c => c.codeSiteKind == 4 
                        && c.extraLevel == 3).ToList();
                        wetTrails = listOfSites.Where(c => c.codeSiteKind == 5 
                        && c.extraLevel == 3).ToList();
                        dryAttractions = listOfSites.Where(c => c.codeSiteKind == 6 
                        && c.extraLevel == 3).ToList();
                        wetAttractions = listOfSites.Where(c => c.codeSiteKind == 7 
                        && c.extraLevel == 3).ToList();
                    }
                        dryTrails = listOfSites.Where(c => c.codeSiteKind == 4 ).ToList();
                        wetTrails = listOfSites.Where(c => c.codeSiteKind == 5 ).ToList();
                        dryAttractions = listOfSites.Where(c => c.codeSiteKind == 6 ).ToList();
                        wetAttractions = listOfSites.Where(c => c.codeSiteKind == 7 ).ToList();
                }
                misLeft = misHours[halfDay_allDay];
                dictOfTrips[i] = new List<Sites>();
                dictOfTrips[i] = SitesRandom(misLeft, numOfSitesOfType, arrListKinds,myAddress);
            }
            return dictOfTrips;

        }
        public List<Sites> SitesRandom(double misHours,double[]arrNumOfSitesToType,
            List<Sites>[] arrListOfSitesToType,string myAddress)
        {
            List<Sites> listToRandom = new List<Sites>();
            List<Sites> result = new List<Sites>();
            result.Add(new Sites() { adress = myAddress });
            while(misHours>0)
            {
                for (int i = 0; i < arrNumOfSitesToType.Length; i++)
                {
                        if (arrNumOfSitesToType[i] > 0)
                        {
                       for(int k=0;k<20;k++)
                        {
                            listToRandom.Add(arrListOfSitesToType[i][random.Next(arrListOfSitesToType[i].Count)]);
                            arrListOfSitesToType[i].Remove(result[result.Count - 1]);
                        }
                        result.Add(ChooseTheShortDistance(listToRandom,result[result.Count-1].adress)); 
                            misHours -= result[result.Count - 1].timeSpend.GetValueOrDefault();
                            arrNumOfSitesToType[i]--;
                        }
                }
            }
            return result;
        }

        private Sites ChooseTheShortDistance(List<Sites> sites,string address)
        {
            Task<Sites> s= GetDistance(address, sites);
            return s.Result;
        }
        public async Task<Sites> GetDistance(string origin, List<Sites> sites)
        {
            Sites s = sites[0];
            string[] locationUrls = { BuildUrlForLocationId(origin.Split()) }, idLocations = new string[20];
            foreach (Sites site in sites)
                locationUrls.Append(BuildUrlForLocationId(site.adress.Split()));
            HttpClient http = new HttpClient();

            for (int i = 0; i < idLocations.Length; i++)
            {
                var responseId = await http.GetAsync(locationUrls[i]);

                if (responseId.IsSuccessStatusCode)
                {
                    var result = await responseId.Content.ReadAsStringAsync();

                    RootLocationBase root = JsonConvert.DeserializeObject<RootLocationBase>(result);
                    idLocations[i] = root.results[0].place_id;
                }
            }
            string url = BuildUrlForDistance(idLocations[0], idLocations[1]);
            for (int i = 2; i < 20; i++)
            {
                url += "|place_id:" + idLocations[i];
            }
            var responseDistance = await http.GetAsync(url);

            if (responseDistance.IsSuccessStatusCode)
            {
                var result = await responseDistance.Content.ReadAsStringAsync();
                RootDistanceBase root = JsonConvert.DeserializeObject<RootDistanceBase>(result);
                string directionMin = root.rows[0].elements[0].distance.text;
                directionMin = directionMin.Replace("mi", "");
                double minDistance = double.Parse(directionMin),min;
                string direction;
                
                for (int i = 1; i < 20; i++)
                {
                    direction = root.rows[0].elements[i].distance.text.Replace("min","");
                    min = double.Parse(direction);
                    if (min < minDistance)
                    {
                        minDistance = min;
                        s = sites[i];
                    }
                }
            }
            return s;
        }
        static string BuildUrlForLocationId(string[] address)
        {
            string location = "";
            string[] locationAsArray;
            locationAsArray =address;

            for (int i = 0; i < locationAsArray.Length; i++)
            {
                if (i < locationAsArray.Length - 1)
                    location += locationAsArray[i] + "+";
                else
                    location += locationAsArray[i];
            }

            return "https://maps.googleapis.com/maps/api/place/textsearch/json?key=AIzaSyCKs4oBHYDXVTUCm-mHhbu7CERTQgbEM2Y&query=" + location + "&mode=driving&units=imperial&sensor=true";
        }

        static string BuildUrlForDistance(string place1, string place2)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?key=AIzaSyCKs4oBHYDXVTUCm-mHhbu7CERTQgbEM2Y&units=imperial&origins=";
            return url + "place_id:" + place1 + "&destinations=place_id:" + place2;
        }
        #endregion

    }
}

