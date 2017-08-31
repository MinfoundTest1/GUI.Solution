using CoreWinSubCommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RxTest
{
    public static class NotificationExtensions
    {
        /// <summary>
        /// Returns an observable sequence of the source any time the <c>PropertyChanged</c> event is raised.
        /// </summary>
        /// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
        /// <param name="source">The object to observe property changes on.</param>
        /// <returns>Returns an observable sequence of the value of the source when ever the <c>PropertyChanged</c> event is raised.</returns>
        public static IObservable<TProperty> PropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
            where T : INotifyPropertyChanged
        {
            return Observable.Create<TProperty>(o =>
            {
                var propertyName = property.GetPropertyInfo().Name;
                var propertySelector = property.Compile();

                return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                                       handler => handler.Invoke,
                                       h => source.PropertyChanged += h,
                                       h => source.PropertyChanged -= h)
                                       .Where(e => e.EventArgs.PropertyName == propertyName)
                                       .Select(e => propertySelector(source))
                                       .Subscribe(o);
            });
        }

        public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var body = property.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Expression is not a property", "property");
            }

            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("Expression is not a property", "property");
            }

            return propertyInfo;
        }

        public static IObservable<TProperty> OnPropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
            where T : INotifyPropertyChanged
        {
            return Observable.Create<TProperty>(o =>
            {
                var propertyName = property.GetPropertyInfo().Name;
                var propertySelector = property.Compile();

                return Observable.Create<PropertyChangedEventArgs>(observer =>
                {
                    PropertyChangedEventHandler handler = (s, e) => observer.OnNext(e);
                    source.PropertyChanged += handler;
                    return Disposable.Create(() => source.PropertyChanged -= handler);
                })
                .Where(e => e.PropertyName == propertyName)
                .Select(e => propertySelector(source))
                .Subscribe(o);
            });
                                       

            //return Observable.Create<PropertyChangedEventArgs>(observer =>
            //{
            //    PropertyChangedEventHandler handler = (s, e) => observer.OnNext(e);
            //    source.PropertyChanged += handler;
            //    return Disposable.Create(() => source.PropertyChanged -= handler);
            //});
        }
    }

    class Person
    {
        public string Name { get; set; }
    }

    class MemberSearchViewModel : BindableObject
    {
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value, "SearchText");
            }
        }

        private Person person;
        public Person Person
        {
            get { return person; }
            set { SetProperty(ref person, value, "Person"); }
        }

        public MemberSearchViewModel()
        {
            //    this.PropertyChanges(vm => vm.SearchText).Subscribe(Search);
      //      this.OnPropertyChanges(vm => vm.SearchText).Subscribe(Search);
        }

        private void Search(string text)
        {
            Console.WriteLine("Search -> {0}", text);
        }
    }
}
