using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace TeleYumaApp.Behaviors
{
    public class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }

    public class EventToCommandBehavior : BehaviorBase<View>
    {
        Delegate eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);


        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }


        //public static readonly BindableProperty RegexValueProperty =
        //   BindableProperty.Create(nameof(RegexValue), typeof(string), typeof(CustomPicker), string.Empty);

        //public string RegexValue
        //{
        //    get { return (string)GetValue(RegexValueProperty); }
        //    set { SetValue(RegexValueProperty, value); }
        //}

        //public static readonly BindableProperty ColorValidProperty =
        //   BindableProperty.Create(nameof(ColorValid), typeof(Color), typeof(CustomPicker), Color.Black);

        //public Color ColorValid
        //{
        //    get { return (Color)GetValue(ColorValidProperty); }
        //    set { SetValue(ColorValidProperty, value); }
        //}

        //public static readonly BindableProperty ColorInvalidProperty =
        //  BindableProperty.Create(nameof(ColorInvalid), typeof(Color), typeof(CustomPicker), Color.Black);

        //public Color ColorInvalid
        //{
        //    get { return (Color)GetValue(ColorInvalidProperty); }
        //    set { SetValue(ColorInvalidProperty, value); }
        //}

        //public static readonly BindableProperty ImageValidProperty =
        //  BindableProperty.Create(nameof(ImageValid), typeof(string), typeof(CustomPicker), string.Empty);

        //public string ImageValid
        //{
        //    get { return (string)GetValue(ImageValidProperty); }
        //    set { SetValue(ImageValidProperty, value); }
        //}

        //public static readonly BindableProperty ImageInvalidProperty =
        //  BindableProperty.Create(nameof(ImageInvalid), typeof(string), typeof(CustomPicker), string.Empty);

        //public string ImageInvalid
        //{
        //    get { return (string)GetValue(ImageInvalidProperty); }
        //    set { SetValue(ImageInvalidProperty, value); }
        //}

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }



        protected override void OnAttachedTo(View bindable)
        {
            //if(bindable is CustomEntry)
            //{
            //    ((CustomEntry)bindable).TextChanged += TextChanged;
            //}

            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        //void TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (RegexValue is null) return;
        //    bool valido = (Regex.IsMatch(e.NewTextValue, RegexValue, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
        //    ((CustomEntry)sender).TextColor = valido ? ColorValid : ColorInvalid;
        //}

        protected override void OnDetachingFrom(View bindable)
        {
            //if (bindable is CustomEntry)
            //{
            //    ((CustomEntry)bindable).TextChanged -= TextChanged;
            //}

            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
            }
            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (eventHandler == null)
            {
                return;
            }
            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
            }
            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
            {
                return;
            }

            object resolvedParameter;
            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
            {
                Command.Execute(resolvedParameter);
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }

    public class ValidateBehavior : BehaviorBase<View>
    {
        Delegate eventHandler;

        public static readonly BindableProperty RegexValueProperty =
            BindableProperty.Create("RegexValue", typeof(string), typeof(ValidateBehavior), string.Empty);

        public string RegexValue
        {
            get { return (string)GetValue(RegexValueProperty); }
            set { SetValue(RegexValueProperty, value); }
        }


        public static readonly BindableProperty ColorValidProperty =
           BindableProperty.Create(nameof(ColorValid), typeof(Color), typeof(ValidateBehavior), Color.Black);

        public Color ColorValid
        {
            get { return (Color)GetValue(ColorValidProperty); }
            set { SetValue(ColorValidProperty, value); }
        }

        public static readonly BindableProperty ColorInvalidProperty =
          BindableProperty.Create(nameof(ColorInvalid), typeof(Color), typeof(ValidateBehavior), Color.Red);

        public Color ColorInvalid
        {
            get { return (Color)GetValue(ColorInvalidProperty); }
            set { SetValue(ColorInvalidProperty, value); }
        }

        public static readonly BindableProperty ImageValidProperty =
          BindableProperty.Create(nameof(ImageValid), typeof(string), typeof(ValidateBehavior), string.Empty);

        public string ImageValid
        {
            get { return (string)GetValue(ImageValidProperty); }
            set { SetValue(ImageValidProperty, value); }
        }

        public static readonly BindableProperty ImageInvalidProperty =
          BindableProperty.Create(nameof(ImageInvalid), typeof(string), typeof(ValidateBehavior), string.Empty);

        public string ImageInvalid
        {
            get { return (string)GetValue(ImageInvalidProperty); }
            set { SetValue(ImageInvalidProperty, value); }
        }


        protected override void OnAttachedTo(View bindable)
        {
            if (bindable is CustomEntry)
            {
                ((CustomEntry)bindable).TextChanged += TextChanged;
            }

            base.OnAttachedTo(bindable);

        }

        void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RegexValue is null) return;
            bool valido = (Regex.IsMatch(e.NewTextValue, RegexValue, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((CustomEntry)sender).TextColor = valido ? ColorValid : ColorInvalid;

            ((CustomEntry)sender).Image = valido ? ImageValid : ImageInvalid;
        }

        protected override void OnDetachingFrom(View bindable)
        {
            if (bindable is CustomEntry)
            {
                ((CustomEntry)bindable).TextChanged -= TextChanged;
            }

            base.OnDetachingFrom(bindable);
        }



    }

}