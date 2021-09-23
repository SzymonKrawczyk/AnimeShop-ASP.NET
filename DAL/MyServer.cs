using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeShop.DAL
{
    public class MyServer
    {

        public static void init() {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        public static SortedDictionary<int, int> GetCart (HttpSessionStateBase session) {

            SortedDictionary<int, int> returnValue;

            if (session["cart"] != null) {

                returnValue = session["cart"] as SortedDictionary<int, int>;
            } else {

                returnValue = new SortedDictionary<int, int>();
            }

            return returnValue;
        }

        public static int getCartSize(HttpSessionStateBase session) {

            var cart = GetCart(session);
            int size = 0;

            foreach (int val in cart.Values) {

                size += val;
            }

            return size;
        }

        public static void removeFromCart(HttpSessionStateBase session, int id) {

            var cart = GetCart(session);

            if (cart.ContainsKey(id)) {
                cart.Remove(id);
                session["cart"] = cart;
                setCartSizeForLayout(session);
            }
        }

        public static void cleanCart(HttpSessionStateBase session) {

            session["cart"] = null;
            setCartSizeForLayout(session);
        }

        public static void addToCart(HttpSessionStateBase session, int id, int amount) {

            var cart = GetCart(session);

            if (cart.ContainsKey(id)) {

                cart[id] = cart[id] + amount;

            } else {

                cart[id] = amount;
            }

            if (cart[id] <= 0) {
                removeFromCart(session, id);
            }


            session["cart"] = cart;

            setCartSizeForLayout(session);
        }

        public static void setCartSizeForLayout(HttpSessionStateBase session) {

            session["cartSize"] = getCartSize(session);
        }


        public static string formatPrice(double value) {

            return string.Format("{0:0.00}", value);
        }

    }

}