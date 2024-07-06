using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Chicken_slayer
{
    [FirestoreData]
    public class BXHstruct
    {
        [FirestoreProperty]

        public string SCORE { get; set; }
    }
}
