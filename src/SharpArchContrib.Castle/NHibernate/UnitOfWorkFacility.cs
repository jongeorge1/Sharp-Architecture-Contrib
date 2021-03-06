using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel;

namespace SharpArchContrib.Castle.NHibernate {
    public class UnitOfWorkFacility : AttributeControlledFacilityBase {
        public UnitOfWorkFacility() : base(typeof(UnitOfWorkInterceptor), LifestyleType.Transient) {}

        protected override List<Attribute> GetAttributes(IHandler handler) {
            var attributes = new List<Attribute>();
            AddClassLevelAttributes(attributes, handler);
            AddMethodLevelAttributes(attributes, handler);

            return attributes;
        }

        private void AddMethodLevelAttributes(List<Attribute> attributes, IHandler handler) {
            foreach (MethodInfo methodInfo in handler.ComponentModel.Implementation.GetMethods()) {
                attributes.AddRange((Attribute[]) methodInfo.GetCustomAttributes(typeof(UnitOfWorkAttribute), false));
            }
        }

        private void AddClassLevelAttributes(List<Attribute> attributes, IHandler handler) {
            attributes.AddRange(
                (Attribute[])
                handler.ComponentModel.Implementation.GetCustomAttributes(typeof(UnitOfWorkAttribute), false));
        }
    }
}