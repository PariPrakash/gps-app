using Android.App;
using Android.Widget;
using Android.OS;

using Android.Content;
using Java.Util;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Android.Runtime;
using Android.Gms.Location;
using Android.Locations;
using System.Threading.Tasks;
using System;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

namespace GPSApp
{
    [Activity(Label = "GPSApp", MainLauncher = true)]
    public class MainActivity : Activity//, ILocationListener
    {
        //TextView txtlatitu;
        //TextView txtlong;
        //Location currentLocation;
        //LocationManager locationManager;
        //string locationProvider;
        //public string TAG
        //{
        //    get;
        //    private set;
        //}
        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);
        //    // Set our view from the "main" layout resource  
        //    SetContentView(Resource.Layout.Main);
        //    txtlatitu = FindViewById<TextView>(Resource.Id.txtlatitude);
        //    txtlong = FindViewById<TextView>(Resource.Id.txtlong);
        //    InitializeLocationManager();
        //}
        //private void InitializeLocationManager()
        //{
        //    locationManager = (LocationManager)GetSystemService(LocationService);
        //    Criteria criteriaForLocationService = new Criteria
        //    {
        //        Accuracy = Accuracy.Fine
        //    };
        //    IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
        //    if (acceptableLocationProviders.Any())
        //    {
        //        locationProvider = acceptableLocationProviders.First();
        //    }
        //    else
        //    {
        //        locationProvider = string.Empty;
        //    }
        //    Log.Debug(TAG, "Using " + locationProvider + ".");
        //}
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        //}
        //protected override void OnPause()
        //{
        //    base.OnPause();
        //    locationManager.RemoveUpdates(this);
        //}
        //public void OnLocationChanged(Location location)
        //{
        //    currentLocation = location;
        //    if (currentLocation == null)
        //    {
        //        //Error Message  
        //    }
        //    else
        //    {
        //        txtlatitu.Text = currentLocation.Latitude.ToString();
        //        txtlong.Text = currentLocation.Longitude.ToString();
        //    }
        //}

        //public void OnProviderDisabled(string provider)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void OnProviderEnabled(string provider)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        //{
        //    throw new System.NotImplementedException();
        //}
        TextView txtLocation;
        Button btnLocation;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            txtLocation = FindViewById<TextView>(Resource.Id.txtLocation);
            btnLocation = FindViewById<Button>(Resource.Id.btnGetLocation);
            //FusedLocationProviderClient client = LocationServices.GetFusedLocationProviderClient(this);
            if (ContextCompat.CheckSelfPermission(this,
                    Manifest.Permission.AccessFineLocation)
            != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this,
                       new String[] { Manifest.Permission.AccessFineLocation },
                       0);
            }
            btnLocation.Click += delegate
            {
                //Task<Location> location = client.GetLastLocationAsync();
                try
                {
                    StartLocationUpdatesAsync();
                }
                catch(Exception e)
                { Console.WriteLine(e.StackTrace);
                }
            };
        }
        MyLocationCallback locationCallback;
        FusedLocationProviderClient client;

        async Task StartLocationUpdatesAsync()
        {
            // Create a callback that will get the location updates
            if (locationCallback == null)
            {
                locationCallback = new MyLocationCallback();
                locationCallback.LocationUpdated += OnLocationResult;
            }

            // Get the current client
            if (client == null)
                client = LocationServices.GetFusedLocationProviderClient(this);

            try
            {
                //Create request and set intervals:
                //Interval: Desired interval for active location updates, it is inexact and you may not receive upates at all if no location servers are available
                //Fastest: Interval is exact and app will never receive updates faster than this value
                var locationRequest = new LocationRequest()
                                          .SetInterval(10000)
                                          .SetFastestInterval(5000)
                                          .SetPriority(LocationRequest.PriorityHighAccuracy);

                await client.RequestLocationUpdatesAsync(locationRequest, locationCallback);
            }
            catch (Exception ex)
            {
                //Handle exception here if failed to register
            }
        }
        void OnLocationResult(object sender, Location location)
        {
            RunOnUiThread(() => {
                txtLocation.Text = "Latitude: " + location.Latitude.ToString() + '\n' +
                "Longitude: " + location.Longitude.ToString();
            });
        }
        class MyLocationCallback : LocationCallback
        {
            public EventHandler<Location> LocationUpdated;
            public override void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);
                LocationUpdated?.Invoke(this, result.LastLocation);
            }
        }
    }
}

