﻿using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.QueryService
{
    [TestFixture]
    public class TopTest
    {
        private static dynamic getDynamicCollection(params dynamic[] items)
        {
            var list = new List<dynamic>();

            if (items != null)
            {
                list.AddRange(items);
            }

            ICollection<KeyValuePair<string, object>> obj = new ExpandoObject();
            obj.Add(new KeyValuePair<string, object>(DataService.COLLECTION_PROPERTY, list));
            return obj;
        }

        private static readonly dynamic _dataEmpty = getDynamicCollection();
        private static readonly dynamic _dataSimple = getDynamicCollection(new {id = 1}, new {id = 2}, new {id = 3});
        
        public static object[] _skipSimpleTestCases =
        {
            new object[] { _dataEmpty,  0, 0 },
            new object[] { _dataEmpty,  0, 1 },
            new object[] { _dataEmpty,  0, 2 },
            
            new object[] { _dataSimple, 0, 0 },
            new object[] { _dataSimple, 1, 1 },
            new object[] { _dataSimple, 2, 2 },
            new object[] { _dataSimple, 3, 3 },
            new object[] { _dataSimple, 3, 4 },
            new object[] { _dataSimple, 3, 5 }
        };
        
        [Test]
        [TestCaseSource(nameof(_skipSimpleTestCases))]
        public void SkipSimpleTest(dynamic data, int expectedCount, int top)
        {
            var service = new Engine.Infrastructure.Services.QueryService(data);

            var results = service.Query(top: top).ToList();
            
            Assert.AreEqual(expectedCount, results.Count);
            for (var i = 0; i < results.Count; i++)
            {
                Assert.AreEqual(data[i].id, results[i].id);
            }
        }
    }
}