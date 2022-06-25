using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Brief summary: Class Model, class is a model for a notes from Firebase firestore
    /// Purpose: of the class is to hold one note
    /// How to use: class holds every element from firebase, it will be set automatically later
    /// </summary>
    [FirestoreData]
    class Model
    {
        [FirestoreProperty]
        public bool added { get; set; }
        [FirestoreProperty]
        public DateTime createDate { get; set; }
        [FirestoreProperty]
        public String id { get; set; }
        [FirestoreProperty]
        public String localisation { get; set; }
        [FirestoreProperty]
        public String notes { get; set; }
        [FirestoreProperty]
        public String title { get; set; }
        [FirestoreProperty]
        public String uid { get; set; }
    }
}
