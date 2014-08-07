using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Esri.Android.Map;
using Com.Esri.Android.Map.Osm;
using Com.Esri.Core.Geometry;
using Com.Esri.Core.Map;
using Com.Esri.Core.Symbol;
using System.Collections.Generic;

namespace BindingTest
{
    [Activity(Label = "TestArcGISForAndroid", MainLauncher = true)]
    public class MainActivity : Activity, View.IOnTouchListener
    {
        MapView mMapView = null;
        GraphicsLayer mGraphicsLayer;
        List<Point> mArrayList;
        bool drawEnabled;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            mMapView = FindViewById<MapView>(Resource.Id.map);
            mMapView.SetEsriLogoVisible(true);
            mMapView.EnableWrapAround(true);

            mGraphicsLayer = new GraphicsLayer();
            mMapView.AddLayer(mGraphicsLayer);

            mArrayList = new List<Point>();

            Button resetButton = FindViewById<Button>(Resource.Id.resetButton);
            Button drawButton = FindViewById<Button>(Resource.Id.drawButton);

            resetButton.Click += (object sender, EventArgs e) =>
            {
                ResetDraw();
            };

            drawButton.Click += (object sender, EventArgs e) =>
            {
                ToggleDraw();
            };
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (drawEnabled)
            {
                mGraphicsLayer.RemoveAll();

                Point point = mMapView.ToMapPoint(new Point(e.GetX(), e.GetY()));
                mArrayList.Add(point);


                if (mArrayList.Count > 1)
                {
                    Polygon polygon = new Polygon();

                    var mArray = mArrayList.ToArray();

                    polygon.StartPath(mArray[0]);
                    for (int i = 1; i < mArray.Length; i++)
                    {
                        polygon.LineTo(mArray[i]);
                    }

                    Graphic graphic = new Graphic(polygon, new SimpleFillSymbol(Android.Graphics.Color.Blue, SimpleFillSymbol.STYLE.Vertical));

                    mGraphicsLayer.AddGraphic(graphic);
                }
            }
            return false;
        }

        public void ToggleDraw()
        {
            if (drawEnabled)
            {
                drawEnabled = false;
                mMapView.SetOnTouchListener(null);
            }
            else
            {
                drawEnabled = true;
                mMapView.SetOnTouchListener(this);
            }
        }

        public void ResetDraw()
        {
            mGraphicsLayer.RemoveAll();
            mArrayList.Clear();
        }
    }
}


