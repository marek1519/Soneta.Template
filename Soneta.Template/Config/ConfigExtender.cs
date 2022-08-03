using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.Db;
using Soneta.Config;
using Soneta.Core;
using Soneta.CRM;
using Soneta.Kadry;
using Soneta.Place;
using Soneta.Tools;
using Soneta.Waluty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soneta.DevUczelnie.Config
{
    public class ConfigExtender : ISessionable
    {

        public ConfigExtender(ISessionable session)
        {
            this.Session = session.Session;
        }

        public bool IsVisible() => true;

        #region MODULES

        private BusinessModule _businessModule;

        public BusinessModule BusinessModul
        {
            get
            {
                if (_businessModule == null)
                    _businessModule = BusinessModule.GetInstance(Session);

                return _businessModule;
            }
        }

        private CoreModule _coreModule;

        public CoreModule CoreModul
        {
            get
            {
                if (_coreModule == null)
                    _coreModule = CoreModule.GetInstance(Session);

                return _coreModule;
            }
        }

        private PlaceModule _placeModule;

        public PlaceModule PlaceModul
        {
            get
            {
                if (_placeModule == null)
                    _placeModule = PlaceModule.GetInstance(Session);

                return _placeModule;
            }
        }

        private KadryModule _kadryModul;

        public KadryModule KadryModul
        {
            get
            {
                if (_kadryModul == null)
                    _kadryModul = KadryModule.GetInstance(Session);

                return _kadryModul;
            }
        }

        private WalutyModule _walutyModul;

        public WalutyModule WalutyModul
        {
            get
            {
                if (_walutyModul == null)
                    _walutyModul = WalutyModule.GetInstance(Session);

                return _walutyModul;
            }
        }



        #endregion

        #region Metody pomocnicze

        /// <summary>
        /// Pobranie dla operatora enova
        /// </summary>
        /// <param name="name"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public string GetOnOperator(string name, bool add = true)
        {
            if (!add) return name;

            return name + Session.Login.Operator.ID;
        }

        /// <summary>
        /// Pobranie dla wszystkich operatorów na danym komputerze
        /// </summary>
        /// <param name="name"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public string GetOnComputer(string name, bool add = true)
        {
            if (!add) return name;
            return name + Environment.MachineName;
        }

        /// <summary>
        ///  Pobranie dla operatora na danym komputerze
        /// </summary>
        /// <param name="name"></param>
        /// <param name="add"></param>
        /// <returns></returns>
        public string GetOnOperatorComputer(string name, bool add = true)
        {
            if (!add) return name;
            return name + Session.Login.Operator.ID + Environment.MachineName;
        }



        [Context]
        public Session Session { get; set; }
        //Metoda odpowiada za ustawianie wartosci parametrów konfiguracji
        public void SetValue<T>(string name, T value, AttributeType type)
        {
            SetValue(Session, name, value, type);
        }

        //Metoda odpowiada za pobieranie wartosci parametrów konfiguracji
        public T GetValue<T>(string name, T def)
        {
            return GetValue(Session, name, def);
        }

        public const string Node = "Soneta.Template";
        public const string SubNode = "DevelConfig";
        //Metoda odpowiada za ustawianie wartosci parametrów konfiguracji
        public static void SetValue<T>(Session session, string _name, T value, AttributeType type)
        {
            string name = Code.ZaszyfrujAlgMD5(_name.ToUpper()).ToUpper();
            using (var t = session.Logout(true))
            {
                var cfgManager = new CfgManager(session);
                //wyszukiwanie gałęzi głównej 
                var node1 = cfgManager.Root.FindSubNode(Node, false) ??
                            cfgManager.Root.AddNode(Node, CfgNodeType.Node);

                //wyszukiwanie liścia 
                var node2 = node1.FindSubNode(SubNode, false) ??
                            node1.AddNode(SubNode, CfgNodeType.Leaf);

                //wyszukiwanie wartosci atrybutu w liściu 
                var attr = node2.FindAttribute(name, false);
                if (attr == null)
                    node2.AddAttribute(name, type, value);
                else
                    attr.Value = value;

                t.CommitUI();
            }
        }

        //Metoda odpowiada za pobieranie wartosci parametrów konfiguracji
        public static T GetValue<T>(Session session, string _name, T def)
        {
            string name = Code.ZaszyfrujAlgMD5(_name.ToUpper()).ToUpper();
            var cfgManager = new CfgManager(session);

            var node1 = cfgManager.Root.FindSubNode(Node, false);

            //Jeśli nie znaleziono gałęzi, zwracamy wartosć domyślną
            if (node1 == null) return def;

            var node2 = node1.FindSubNode(SubNode, false);
            if (node2 == null) return def;

            var attr = node2.FindAttribute(name, false);
            if (attr == null) return def;

            if (attr.Value == null) return def;

            return (T)attr.Value;
        }

        /// <summary>
        /// Pobranie nazwy do max dlugosci
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetNameToLength(string name)
        {
            if (name.Length <= 40) return name;

            return name.Substring(0, 40);
        }

        #endregion Metody pomocnicze
    }
}
