using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chicken_slayer
{
    [FirestoreData]
    internal class BXHdata
    {
        [FirestoreProperty]
        public string NAME {  get; set; }
        [FirestoreProperty]
        public string SCORE { get; set; }
    }
}
