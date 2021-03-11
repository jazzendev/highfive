using HighFive.Core.DomainModel;
using HighFive.Web.Portal.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.FakeData
{
    public class FakeTenant
    {
        private static List<TenantApiViewModel> _data;

        static FakeTenant()
        {
            #region init data
            _data = new List<TenantApiViewModel>() {
new TenantApiViewModel () {Id="5ff59866fc13ae2459000000",ParentId="5ff59866fc13ae2459000001",RootId="5ff59866fc13ae2459000002",Name="Dabtype",Domain="twitter.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000003",ParentId="5ff59866fc13ae2459000004",RootId="5ff59866fc13ae2459000005",Name="Viva",Domain="squarespace.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000006",ParentId="5ff59866fc13ae2459000007",RootId="5ff59866fc13ae2459000008",Name="Babbleset",Domain="shareasale.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000009",ParentId="5ff59866fc13ae245900000a",RootId="5ff59866fc13ae245900000b",Name="Thoughtstorm",Domain="ifeng.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900000c",ParentId="5ff59866fc13ae245900000d",RootId="5ff59866fc13ae245900000e",Name="Trunyx",Domain="github.io" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900000f",ParentId="5ff59866fc13ae2459000010",RootId="5ff59866fc13ae2459000011",Name="Fiveclub",Domain="washingtonpost.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000012",ParentId="5ff59866fc13ae2459000013",RootId="5ff59866fc13ae2459000014",Name="Voolia",Domain="bluehost.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000015",ParentId="5ff59866fc13ae2459000016",RootId="5ff59866fc13ae2459000017",Name="Jaloo",Domain="ox.ac.uk" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000018",ParentId="5ff59866fc13ae2459000019",RootId="5ff59866fc13ae245900001a",Name="Plajo",Domain="dropbox.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900001b",ParentId="5ff59866fc13ae245900001c",RootId="5ff59866fc13ae245900001d",Name="Gabcube",Domain="tripod.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900001e",ParentId="5ff59866fc13ae245900001f",RootId="5ff59866fc13ae2459000020",Name="Skaboo",Domain="tinypic.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000021",ParentId="5ff59866fc13ae2459000022",RootId="5ff59866fc13ae2459000023",Name="Devpoint",Domain="webnode.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000024",ParentId="5ff59866fc13ae2459000025",RootId="5ff59866fc13ae2459000026",Name="Mycat",Domain="whitehouse.gov" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000027",ParentId="5ff59866fc13ae2459000028",RootId="5ff59866fc13ae2459000029",Name="Skiba",Domain="smh.com.au" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900002a",ParentId="5ff59866fc13ae245900002b",RootId="5ff59866fc13ae245900002c",Name="Roombo",Domain="livejournal.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900002d",ParentId="5ff59866fc13ae245900002e",RootId="5ff59866fc13ae245900002f",Name="Topicware",Domain="dailymotion.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000030",ParentId="5ff59866fc13ae2459000031",RootId="5ff59866fc13ae2459000032",Name="Abata",Domain="spiegel.de" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000033",ParentId="5ff59866fc13ae2459000034",RootId="5ff59866fc13ae2459000035",Name="Brainlounge",Domain="hugedomains.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000036",ParentId="5ff59866fc13ae2459000037",RootId="5ff59866fc13ae2459000038",Name="Pixonyx",Domain="yellowbook.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000039",ParentId="5ff59866fc13ae245900003a",RootId="5ff59866fc13ae245900003b",Name="Pixope",Domain="joomla.org" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900003c",ParentId="5ff59866fc13ae245900003d",RootId="5ff59866fc13ae245900003e",Name="Topiclounge",Domain="tumblr.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900003f",ParentId="5ff59866fc13ae2459000040",RootId="5ff59866fc13ae2459000041",Name="Camido",Domain="amazonaws.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000042",ParentId="5ff59866fc13ae2459000043",RootId="5ff59866fc13ae2459000044",Name="Meezzy",Domain="mozilla.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000045",ParentId="5ff59866fc13ae2459000046",RootId="5ff59866fc13ae2459000047",Name="Nlounge",Domain="about.me" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000048",ParentId="5ff59866fc13ae2459000049",RootId="5ff59866fc13ae245900004a",Name="Avaveo",Domain="seattletimes.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900004b",ParentId="5ff59866fc13ae245900004c",RootId="5ff59866fc13ae245900004d",Name="Aivee",Domain="yelp.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900004e",ParentId="5ff59866fc13ae245900004f",RootId="5ff59866fc13ae2459000050",Name="Centizu",Domain="seattletimes.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000051",ParentId="5ff59866fc13ae2459000052",RootId="5ff59866fc13ae2459000053",Name="Flashpoint",Domain="ihg.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000054",ParentId="5ff59866fc13ae2459000055",RootId="5ff59866fc13ae2459000056",Name="Flipstorm",Domain="jimdo.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000057",ParentId="5ff59866fc13ae2459000058",RootId="5ff59866fc13ae2459000059",Name="Leenti",Domain="dailymotion.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900005a",ParentId="5ff59866fc13ae245900005b",RootId="5ff59866fc13ae245900005c",Name="Omba",Domain="webmd.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900005d",ParentId="5ff59866fc13ae245900005e",RootId="5ff59866fc13ae245900005f",Name="Yacero",Domain="xing.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000060",ParentId="5ff59866fc13ae2459000061",RootId="5ff59866fc13ae2459000062",Name="Jabbertype",Domain="wikispaces.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000063",ParentId="5ff59866fc13ae2459000064",RootId="5ff59866fc13ae2459000065",Name="Youspan",Domain="sogou.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000066",ParentId="5ff59866fc13ae2459000067",RootId="5ff59866fc13ae2459000068",Name="Voomm",Domain="examiner.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000069",ParentId="5ff59866fc13ae245900006a",RootId="5ff59866fc13ae245900006b",Name="Zazio",Domain="google.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900006c",ParentId="5ff59866fc13ae245900006d",RootId="5ff59866fc13ae245900006e",Name="Gigazoom",Domain="ted.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900006f",ParentId="5ff59866fc13ae2459000070",RootId="5ff59866fc13ae2459000071",Name="Vinder",Domain="dropbox.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000072",ParentId="5ff59866fc13ae2459000073",RootId="5ff59866fc13ae2459000074",Name="Gigaclub",Domain="pinterest.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000075",ParentId="5ff59866fc13ae2459000076",RootId="5ff59866fc13ae2459000077",Name="Brainbox",Domain="sciencedirect.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000078",ParentId="5ff59866fc13ae2459000079",RootId="5ff59866fc13ae245900007a",Name="Gigaclub",Domain="dailymail.co.uk" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900007b",ParentId="5ff59866fc13ae245900007c",RootId="5ff59866fc13ae245900007d",Name="Realbridge",Domain="yolasite.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900007e",ParentId="5ff59866fc13ae245900007f",RootId="5ff59866fc13ae2459000080",Name="Blogpad",Domain="blogger.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000081",ParentId="5ff59866fc13ae2459000082",RootId="5ff59866fc13ae2459000083",Name="Livepath",Domain="furl.net" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000084",ParentId="5ff59866fc13ae2459000085",RootId="5ff59866fc13ae2459000086",Name="Photobug",Domain="indiegogo.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000087",ParentId="5ff59866fc13ae2459000088",RootId="5ff59866fc13ae2459000089",Name="Centidel",Domain="washingtonpost.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900008a",ParentId="5ff59866fc13ae245900008b",RootId="5ff59866fc13ae245900008c",Name="Skajo",Domain="yellowpages.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900008d",ParentId="5ff59866fc13ae245900008e",RootId="5ff59866fc13ae245900008f",Name="Brightdog",Domain="thetimes.co.uk" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000090",ParentId="5ff59866fc13ae2459000091",RootId="5ff59866fc13ae2459000092",Name="Zoovu",Domain="meetup.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000093",ParentId="5ff59866fc13ae2459000094",RootId="5ff59866fc13ae2459000095",Name="Skyvu",Domain="vk.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000096",ParentId="5ff59866fc13ae2459000097",RootId="5ff59866fc13ae2459000098",Name="Innotype",Domain="qq.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000099",ParentId="5ff59866fc13ae245900009a",RootId="5ff59866fc13ae245900009b",Name="Zoombeat",Domain="soundcloud.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900009c",ParentId="5ff59866fc13ae245900009d",RootId="5ff59866fc13ae245900009e",Name="Dabtype",Domain="blogtalkradio.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900009f",ParentId="5ff59866fc13ae24590000a0",RootId="5ff59866fc13ae24590000a1",Name="Linklinks",Domain="bravesites.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000a2",ParentId="5ff59866fc13ae24590000a3",RootId="5ff59866fc13ae24590000a4",Name="Quatz",Domain="census.gov" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000a5",ParentId="5ff59866fc13ae24590000a6",RootId="5ff59866fc13ae24590000a7",Name="Oyonder",Domain="time.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000a8",ParentId="5ff59866fc13ae24590000a9",RootId="5ff59866fc13ae24590000aa",Name="Photobug",Domain="cnbc.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000ab",ParentId="5ff59866fc13ae24590000ac",RootId="5ff59866fc13ae24590000ad",Name="Buzzster",Domain="google.ru" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000ae",ParentId="5ff59866fc13ae24590000af",RootId="5ff59866fc13ae24590000b0",Name="Jabbercube",Domain="google.com.hk" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000b1",ParentId="5ff59866fc13ae24590000b2",RootId="5ff59866fc13ae24590000b3",Name="Quatz",Domain="oracle.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000b4",ParentId="5ff59866fc13ae24590000b5",RootId="5ff59866fc13ae24590000b6",Name="Quimm",Domain="thetimes.co.uk" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000b7",ParentId="5ff59866fc13ae24590000b8",RootId="5ff59866fc13ae24590000b9",Name="Gabtype",Domain="ocn.ne.jp" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000ba",ParentId="5ff59866fc13ae24590000bb",RootId="5ff59866fc13ae24590000bc",Name="Brainverse",Domain="columbia.edu" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000bd",ParentId="5ff59866fc13ae24590000be",RootId="5ff59866fc13ae24590000bf",Name="Abata",Domain="arstechnica.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000c0",ParentId="5ff59866fc13ae24590000c1",RootId="5ff59866fc13ae24590000c2",Name="Zazio",Domain="github.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000c3",ParentId="5ff59866fc13ae24590000c4",RootId="5ff59866fc13ae24590000c5",Name="Ooba",Domain="example.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000c6",ParentId="5ff59866fc13ae24590000c7",RootId="5ff59866fc13ae24590000c8",Name="Midel",Domain="admin.ch" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000c9",ParentId="5ff59866fc13ae24590000ca",RootId="5ff59866fc13ae24590000cb",Name="Cogibox",Domain="hao123.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000cc",ParentId="5ff59866fc13ae24590000cd",RootId="5ff59866fc13ae24590000ce",Name="Yotz",Domain="hostgator.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000cf",ParentId="5ff59866fc13ae24590000d0",RootId="5ff59866fc13ae24590000d1",Name="Teklist",Domain="macromedia.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000d2",ParentId="5ff59866fc13ae24590000d3",RootId="5ff59866fc13ae24590000d4",Name="Roombo",Domain="icq.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000d5",ParentId="5ff59866fc13ae24590000d6",RootId="5ff59866fc13ae24590000d7",Name="Feedfire",Domain="linkedin.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000d8",ParentId="5ff59866fc13ae24590000d9",RootId="5ff59866fc13ae24590000da",Name="Izio",Domain="bloomberg.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000db",ParentId="5ff59866fc13ae24590000dc",RootId="5ff59866fc13ae24590000dd",Name="Zava",Domain="abc.net.au" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000de",ParentId="5ff59866fc13ae24590000df",RootId="5ff59866fc13ae24590000e0",Name="Rhynoodle",Domain="acquirethisname.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000e1",ParentId="5ff59866fc13ae24590000e2",RootId="5ff59866fc13ae24590000e3",Name="Babbleopia",Domain="github.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000e4",ParentId="5ff59866fc13ae24590000e5",RootId="5ff59866fc13ae24590000e6",Name="Quimm",Domain="bloglines.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000e7",ParentId="5ff59866fc13ae24590000e8",RootId="5ff59866fc13ae24590000e9",Name="Eidel",Domain="smugmug.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000ea",ParentId="5ff59866fc13ae24590000eb",RootId="5ff59866fc13ae24590000ec",Name="BlogXS",Domain="typepad.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000ed",ParentId="5ff59866fc13ae24590000ee",RootId="5ff59866fc13ae24590000ef",Name="Midel",Domain="kickstarter.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000f0",ParentId="5ff59866fc13ae24590000f1",RootId="5ff59866fc13ae24590000f2",Name="Livepath",Domain="lycos.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000f3",ParentId="5ff59866fc13ae24590000f4",RootId="5ff59866fc13ae24590000f5",Name="Roombo",Domain="sourceforge.net" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000f6",ParentId="5ff59866fc13ae24590000f7",RootId="5ff59866fc13ae24590000f8",Name="Skinte",Domain="reddit.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000f9",ParentId="5ff59866fc13ae24590000fa",RootId="5ff59866fc13ae24590000fb",Name="Edgeclub",Domain="liveinternet.ru" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000fc",ParentId="5ff59866fc13ae24590000fd",RootId="5ff59866fc13ae24590000fe",Name="Agivu",Domain="hud.gov" },
new TenantApiViewModel () {Id="5ff59866fc13ae24590000ff",ParentId="5ff59866fc13ae2459000100",RootId="5ff59866fc13ae2459000101",Name="Gabvine",Domain="google.it" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000102",ParentId="5ff59866fc13ae2459000103",RootId="5ff59866fc13ae2459000104",Name="Blogspan",Domain="slideshare.net" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000105",ParentId="5ff59866fc13ae2459000106",RootId="5ff59866fc13ae2459000107",Name="Realbuzz",Domain="purevolume.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000108",ParentId="5ff59866fc13ae2459000109",RootId="5ff59866fc13ae245900010a",Name="Browsetype",Domain="uol.com.br" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900010b",ParentId="5ff59866fc13ae245900010c",RootId="5ff59866fc13ae245900010d",Name="Browsebug",Domain="weather.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900010e",ParentId="5ff59866fc13ae245900010f",RootId="5ff59866fc13ae2459000110",Name="Trunyx",Domain="goodreads.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000111",ParentId="5ff59866fc13ae2459000112",RootId="5ff59866fc13ae2459000113",Name="Edgetag",Domain="mapy.cz" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000114",ParentId="5ff59866fc13ae2459000115",RootId="5ff59866fc13ae2459000116",Name="Skiba",Domain="cbsnews.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000117",ParentId="5ff59866fc13ae2459000118",RootId="5ff59866fc13ae2459000119",Name="Voonix",Domain="accuweather.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900011a",ParentId="5ff59866fc13ae245900011b",RootId="5ff59866fc13ae245900011c",Name="Roodel",Domain="facebook.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae245900011d",ParentId="5ff59866fc13ae245900011e",RootId="5ff59866fc13ae245900011f",Name="Aimbo",Domain="topsy.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000120",ParentId="5ff59866fc13ae2459000121",RootId="5ff59866fc13ae2459000122",Name="Realfire",Domain="topsy.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000123",ParentId="5ff59866fc13ae2459000124",RootId="5ff59866fc13ae2459000125",Name="Eidel",Domain="youtube.com" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000126",ParentId="5ff59866fc13ae2459000127",RootId="5ff59866fc13ae2459000128",Name="Tazzy",Domain="last.fm" },
new TenantApiViewModel () {Id="5ff59866fc13ae2459000129",ParentId="5ff59866fc13ae245900012a",RootId="5ff59866fc13ae245900012b",Name="Tanoodle",Domain="telegraph.co.uk" }
#endregion
            };
        }


        public static List<TenantApiViewModel> GetData()
        {
            return _data;
        }

        public static TenantApiViewModel Find(string id) {
            return _data.FirstOrDefault(f => f.Id == id);
        }

        public static List<TenantApiViewModel> Query(int page, int size)
        {
            return _data.Skip(page * size).Take(size).ToList();
        }

        public static PaginationResult<TenantApiViewModel> Query(string keyword, int page, int size)
        {
            var data = _data
                .Where(d => { return string.IsNullOrEmpty(keyword) ? true : d.Name.Contains(keyword); })
                .Skip(page * size)
                .Take(size)
                .ToList();

            return new PaginationResult<TenantApiViewModel>()
            {
                Data = data,
                Page = page,
                Size = size,
                Total = _data.Count
            };
        }

        public static TenantApiViewModel Insert(TenantApiViewModel model) {
            _data.Add(model);
            return model;
        }
        public static TenantApiViewModel Update(TenantApiViewModel model)
        {
            var m = _data.FirstOrDefault(f => f.Id == model.Id);
            m = model;
            return model;
        }
    }
}
