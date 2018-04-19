using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace _4600Project
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateWithString()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");

            Assert.AreEqual("teststring", q.QueryUrl);
        }

        [TestMethod]
        public void TestCreateWithUrl()
        {
            TwitterQuery q = TwitterQuery.Create("https://twitter.com/4600Project");

            Assert.AreEqual("https://twitter.com/4600Project", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterWithTwoStrings()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", "Valuestring");

            List<KeyValuePair<string,string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "Valuestring"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=Valuestring", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterWithStringAndInt()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", 3);

            List<KeyValuePair<string, string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "3"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=3", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterWithStringAndDouble()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", 3.5);

            List<KeyValuePair<string, string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "3.5"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=3.5", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterTwiceWithTwoStrings()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", "Valuestring");
            q.AddParameter("Keystring2", "Valuestring2");

            List<KeyValuePair<string, string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "Valuestring"));
            l.Add(new KeyValuePair<string, string>("Keystring2", "Valuestring2"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=Valuestring&Keystring2=Valuestring2", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterTwiceWithInt()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", 3);
            q.AddParameter("Keystring2", 4);

            List<KeyValuePair<string, string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "3"));
            l.Add(new KeyValuePair<string, string>("Keystring2", "4"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=3&Keystring2=4", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterTwiceWithDouble()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", 3.5);
            q.AddParameter("Keystring2", 4.2);

            List<KeyValuePair<string, string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "3.5"));
            l.Add(new KeyValuePair<string, string>("Keystring2", "4.2"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=3.5&Keystring2=4.2", q.QueryUrl);
        }

        [TestMethod]
        public void TestAddParamaterTwiceWithDoubleAndInt()
        {
            TwitterQuery q = TwitterQuery.Create("teststring");
            q.AddParameter("Keystring", 3.5);
            q.AddParameter("Keystring2", 4);

            List<KeyValuePair<string, string>> l = new List<KeyValuePair<string, string>>();
            l.Add(new KeyValuePair<string, string>("Keystring", "3.5"));
            l.Add(new KeyValuePair<string, string>("Keystring2", "4"));

            CollectionAssert.AreEqual(l, q.QueryParameterList);
            Assert.AreEqual("teststring?Keystring=3.5&Keystring2=4", q.QueryUrl);
        }
    }
}
