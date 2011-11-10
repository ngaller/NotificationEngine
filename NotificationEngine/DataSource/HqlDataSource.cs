﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Sage.Platform.Orm;

namespace NotificationEngine.DataSource
{
    /// <summary>
    /// Runs HQL query.
    /// Limitation right now:
    ///  * you have to have at least 2 fields in qry
    ///  * you have to assign an alias to each
    /// </summary>
    public class HqlDataSource : IWorkItemDataSource
    {
        private String _query;
        private List<string> _fields;

        public IEnumerable<IRecord> Read(Interfaces.WorkItem wo)
        {
            List<String> fields = GetFields();
            using (var sess = new SessionScopeWrapper())
            {
                var qry = sess.CreateQuery(_query);
                foreach (object o in qry.List())
                {
                    yield return new HqlRecordWrapper(o is object[] ? (object[])o : new object[] { o },
                        fields);
                }
            }
        }

        public List<string> GetFields()
        {
            if (_fields != null)
                return _fields;
            _fields = new List<string>();
            using (var sess = new SessionScopeWrapper())
            {
                var qry = sess.CreateQuery(_query);
                _fields = qry.ReturnAliases.ToList();
            }
            return _fields;
        }

        public string EscapeLiteral(string literalValue)
        {
            throw new NotImplementedException();
        }

        public void LoadConfiguration(string configBlob)
        {
            throw new NotImplementedException();
        }
    }
}
