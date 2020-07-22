using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TeleYumaApp
{
    public class ExpandableEditor : Editor
    {
        public ExpandableEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~ExpandableEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }

    public class CustomPicker : Picker
    {
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(CustomPicker), string.Empty);
        
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }

    public class CustomTabbedPage : TabbedPage
    {

    }

    public class ImageCircle : Image
    {
        public ImageCircle() : base()
        {
            const int _animationTime = 2;
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (t) => {
                    await this.FadeTo(0.5, 150, Easing.CubicOut);
                    await this.ScaleTo(0.90, 150, Easing.CubicOut);
                    await this.FadeTo(1, 150, Easing.CubicIn);
                    await this.ScaleTo(1, 50, Easing.CubicIn);
                })
            });
        }
    }

   public enum AutoScrollDirection
    {
        Top,
        Bottom
    }

    public class ScrollableListView : ListView
    {
        #region AutoScrollDirection AutoScrollDirection [BindableProperty]
        public static readonly BindableProperty AutoScrollDirectionProperty =
             BindableProperty.CreateAttached(
                  "AutoScrollDirection",
                  typeof(AutoScrollDirection),
                  typeof(AutoScrollDirection),
                  AutoScrollDirection.Bottom,
                  BindingMode.OneWay);

        public AutoScrollDirection AutoScrollDirection
        {
            get { return (AutoScrollDirection)GetValue(AutoScrollDirectionProperty); }
            set { SetValue(AutoScrollDirectionProperty, value); }
        }
        #endregion

        public event EventHandler LongClicked;
        public void OnLongClicked(object item, int index)
        {
            LongClicked?.Invoke(this, new ItemTappedEventArgs(this, item, index));
        }

        #region string AddedItemMessagingCenterMessageKey [BindableProperty]
        public static readonly BindableProperty AddedItemMessagingCenterMessageKeyProperty =
             BindableProperty.CreateAttached(
                  "AddedItemMessagingCenterMessageKey",
                  typeof(string),
                  typeof(string),
                string.Empty,
                BindingMode.OneWay);

        public string AddedItemMessagingCenterMessageKey
        {
            get { return (string)GetValue(AddedItemMessagingCenterMessageKeyProperty); }
            set { SetValue(AddedItemMessagingCenterMessageKeyProperty, value); }
        }
        #endregion

        #region bool AutoDeselectItem [BindableProperty]
        public static readonly BindableProperty AutoDeselectItemProperty =
             BindableProperty.CreateAttached(
                  "AutoDeselectItem",
                  typeof(bool),
                  typeof(bool),
                  true,
                  BindingMode.OneWay);

        public bool AutoDeselectItem
        {
            get { return (bool)GetValue(AutoDeselectItemProperty); }
            set { SetValue(AutoDeselectItemProperty, value); }
        }
        #endregion

        private object _lastItemAdded;
        private bool _isSubscribedAddedItemMessagingCenterMessageKey;
        private string _lastSubscribedAddedItemMessagingCenterMessageKeyName;

        public ScrollableListView()
        {
           
            this.LongPressAction = LongPressActionEvent;

            this.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                if (AutoDeselectItem)
                {
                    this.SelectedItem = null; // this deselects the colour of the previously selected item.
                }
            };


            this.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == ScrollableListView.ItemsSourceProperty.PropertyName)
                {
                    if (_lastItemAdded != null)
                    {
                        switch (AutoScrollDirection)
                        {
                            case AutoScrollDirection.Top:
                                this.ScrollTo(_lastItemAdded, ScrollToPosition.Start, true); // makes sure, when displaying that item, it shows the top of it
                                break;

                            case AutoScrollDirection.Bottom:
                                this.ScrollTo(_lastItemAdded, ScrollToPosition.End, true); // makes sure, when displaying that item, it shows the bottom of it
                                break;
                        }
                        _lastItemAdded = null;
                    }
                }

                if (e.PropertyName == ScrollableListView.AddedItemMessagingCenterMessageKeyProperty.PropertyName)
                {
                    // as this property is bindable (hence may change), and as assignment of this property 
                    // causes a MessagingCenter subscription, we must always unassign it if assigned previously.

                    if (_isSubscribedAddedItemMessagingCenterMessageKey)
                    {
                        UnSubscribeLastAddedItemMessagingCenterMessageKey();
                    }

                    if (!string.IsNullOrEmpty(AddedItemMessagingCenterMessageKey)) // It is left unsubscribed if value is string.Empty.
                    {
                        SubscribeAddedItemMessagingCenterMessageKey();
                    }
                }
            };
        }

        private void LongPressActionEvent()
        {
            var item = this.SelectedItem;
        }

        public Action LongPressAction;


        private void UnSubscribeLastAddedItemMessagingCenterMessageKey()
        {
            MessagingCenter.Unsubscribe<object, object>(this, _lastSubscribedAddedItemMessagingCenterMessageKeyName);

            _isSubscribedAddedItemMessagingCenterMessageKey = false;
            _lastSubscribedAddedItemMessagingCenterMessageKeyName = string.Empty;
        }

        //private void SubscribeAddedItemMessagingCenterMessageKey()
        //{
        //    MessagingCenter.Subscribe<object, object>(this, AddedItemMessagingCenterMessageKey, (sender, arg) =>
        //    {
        //        switch (AutoScrollDirection)
        //        {
        //            case AutoScrollDirection.Top:
        //                this.ScrollTo(arg, ScrollToPosition.Start, true); // makes sure, when displaying that item, it shows the top of it
        //                break;

        //            case AutoScrollDirection.Bottom:
        //                this.ScrollTo(arg, ScrollToPosition.End, true); // makes sure, when displaying that item, it shows the bottom of it
        //                break;
        //        }

        //    });

        //    _isSubscribedAddedItemMessagingCenterMessageKey = true;
        //    _lastSubscribedAddedItemMessagingCenterMessageKeyName = AddedItemMessagingCenterMessageKey;
        //}

        private void SubscribeAddedItemMessagingCenterMessageKey()
        {
            MessagingCenter.Subscribe<object, object>(this, AddedItemMessagingCenterMessageKey, (sender, arg) =>
            {
                _lastItemAdded = arg;
            });

            _isSubscribedAddedItemMessagingCenterMessageKey = true;
            _lastSubscribedAddedItemMessagingCenterMessageKeyName = AddedItemMessagingCenterMessageKey;
        }
    }

    /// <summary>
    /// Long pressed effect. Used for invoking commands on long press detection cross platform
    /// </summary>
    public class LongPressedEffect : RoutingEffect
    {
        public LongPressedEffect() : base("TeleYumaApp.LongPressedEffect")
        {
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(LongPressedEffect), (object)null);
        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }

        public static void SetCommand(BindableObject view, ICommand value)
        {
            view.SetValue(CommandProperty, value);
        }


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(LongPressedEffect), (object)null);
        public static object GetCommandParameter(BindableObject view)
        {
            return view.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject view, object value)
        {
            view.SetValue(CommandParameterProperty, value);
        }
    }

    public class CustomListView : ListView
    {
        public event EventHandler LongClicked;
        public void OnLongClicked(object item,int index)
        {
            LongClicked?.Invoke(this, new ItemTappedEventArgs(this,item,index));
        }
    }
}
