
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace TeleYumaApp.Behaviors
{
    public static class cons
    {
        public static Color validColor = Color.Black;
        public static Color invalidColor = Color.Red;
    }

    public class EmailValidatorBehavior : Behavior<Entry>
    {
        const string emailRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        // Valida si el texto introducido es un correo electrónico
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = (Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((Entry)sender).TextColor = valido ? cons.validColor : cons.invalidColor;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }

    public class PasswordValidatorBehavior : Behavior<Entry>
    {
        // "^([a-zA-Z0-9]{4,16})$";
        //@"(?=^.{6,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";
        const string PasswordRegex = "^([a-zA-Z0-9]{6,16})$";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = false;
            bool Upper = e.NewTextValue.Any(c => char.IsUpper(c));
            bool Lower = e.NewTextValue.Any(c => char.IsLower(c));
            bool Digit = e.NewTextValue.Any(c => char.IsDigit(c));
            if (Upper && Lower && Digit)
                valido = true;
            /*(Regex.IsMatch(e.NewTextValue, PasswordRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));*/
            ((Entry)sender).TextColor = valido ? cons.validColor : cons.invalidColor;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }

    public class NumeroValidatorBehavior : Behavior<Entry>
    {
        const string digitosRegEx = @"^[0-9]+$";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        // Solo dígitos
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = (Regex.IsMatch(e.NewTextValue, digitosRegEx, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((Entry)sender).TextColor = valido ? cons.validColor : cons.invalidColor;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }

    public class LetraValidatorBehavior : Behavior<Entry>
    {
        const string letrasRegEx = @"^[a-zA-Z]+$";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        // Solo dígitos
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = (Regex.IsMatch(e.NewTextValue, letrasRegEx, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((Entry)sender).TextColor = valido ? cons.validColor : cons.invalidColor;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }

    public class EntryMaxLengthValidatorBehavior : Behavior<Entry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            // if Entry text is longer then valid length

            if (entry.Text.Length > this.MaxLength)
            {
                string entryText = entry.Text;

                entryText = entryText.Remove(entryText.Length - 1); // remove last char

                entry.Text = entryText;
            }
        }
    }

    /// <summary>
    /// Material entry length behavior. Allows for the limitation of the text with a min and max length
    /// </summary>
   
}
