using Google.Cloud.Firestore;
using Service.Exceptions;
using Service.IServices;
using ShopRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class FirebaseService<T>:IFirebaseService<T>
    {
        FirestoreDb dbFirestore;
        public FirebaseService() 
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"serviceAccountKey.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            dbFirestore = FirestoreDb.Create("orchid-6cf91");
        }


        public async Task<string> Save(Object saveObj, long id, string collectionName)
        {
            try
            {
                DocumentReference docRef = dbFirestore.Collection(collectionName).Document(id.ToString());
                await docRef.SetAsync(saveObj);

                return (await docRef.GetSnapshotAsync()).UpdateTime.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving document: {e.Message}");
                throw;
            }
        }

        public async Task<T> GetByKey(string key, string collectionName)
        {
            try
            {
                DocumentReference docRef = dbFirestore.Collection(collectionName).Document(key);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    return snapshot.ConvertTo<T>();
                }
                else
                {
                    throw new NotFoundException($"Document with key '{key}' not found in collection '{collectionName}'.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting document: {e.Message}");
                throw;
            }
        }

        public async Task<Auction> GetAuctionByKey(string key, string collectionName)
        {
            try
            {
                DocumentReference docRef = dbFirestore.Collection(collectionName).Document(key);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    return snapshot.ConvertTo<Auction>();
                }
                else
                {
                    throw new NotFoundException($"Document with key '{key}' not found in collection '{collectionName}'.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting document: {e.Message}");
                throw;
            }
        }

        public async Task<bool> Delete(string key, string collectionName)
        {
            try
            {
                WriteResult result = await dbFirestore.Collection(collectionName).Document(key).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting document: {e.Message}");
                throw;
            }
        }

        public async Task<List<T>> Get(string collectionName)
        {
            try
            {
                QuerySnapshot snapshot = await dbFirestore.Collection(collectionName).GetSnapshotAsync();
                List<T> objects = snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
                return objects;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting auctions: {e.Message}");
                throw;
            }
        }

        public async Task<List<Auction>> GetAuctions(string collectionName, int? status = -1, DateTime? time = null)
        {
            try
            {
                Query query = dbFirestore.Collection(collectionName);

                if (status > -1)
                {
                    query = query.WhereEqualTo("Status", status);
                }
                if (time.HasValue)
                {
                    //DateTime utcNow = time.;

                    query = query.WhereLessThan("StartDate", time.Value);
                }
                

                /*QuerySnapshot snapshot = await dbFirestore.Collection(collectionName).GetSnapshotAsync();
                List<Auction> objects = snapshot.Documents.Select(doc => doc.ConvertTo<Auction>()).ToList();*/
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                List<Auction> objects = snapshot.Documents.Select(doc => doc.ConvertTo<Auction>()).ToList();

                return objects;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting auctions: {e.Message}");
                throw;
            }
        }


    }
}
