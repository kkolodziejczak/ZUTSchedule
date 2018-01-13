using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace ZUTSchedule.mobile
{
    public class SwipeListener : PanGestureRecognizer
    {
        private ISwipeCallBack _ISwipeCallback;
        private double translatedX = 0, translatedY = 0;

        public SwipeListener(View view, ISwipeCallBack iSwipeCallBack)
        {
            _ISwipeCallback = iSwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {

            View Content = (View)sender;

            switch (e.StatusType)
            {

                case GestureStatus.Running:

                    try
                    {
                        translatedX = e.TotalX;
                        translatedY = e.TotalY;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    break;

                case GestureStatus.Completed:

                    Debug.WriteLine("translatedX : " + translatedX);
                    Debug.WriteLine("translatedY : " + translatedY);

                    if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                    {
                        _ISwipeCallback.OnLeftSwipe(Content);
                    }
                    else if (translatedX > 0 && translatedX > Math.Abs(translatedY))
                    {
                        _ISwipeCallback.OnRightSwipe(Content);
                    }
                    else if (translatedY < 0 && Math.Abs(translatedY) > Math.Abs(translatedX))
                    {
                        _ISwipeCallback.OnTopSwipe(Content);
                    }
                    else if (translatedY > 0 && translatedY > Math.Abs(translatedX))
                    {
                        _ISwipeCallback.OnBottomSwipe(Content);
                    }
                    else
                    {
                        _ISwipeCallback.OnNothingSwiped(Content);
                    }

                    break;

            }
        }

    }
}
