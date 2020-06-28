// <copyright file="IServiceLocator.cs" company="Beckman Coulter">
// Copyright (c) Beckman Coulter. All rights reserved.
// </copyright>

namespace Authoring.Infrastructure.ServiceLocator
{
    /// <summary>
    /// Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// The identity matches with a service scope instance.
        /// </summary>
        int ScopeId { get; set; }

        /// <summary>
        /// Get service of type T.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type T or null if there is no such service.</returns>
        T GetService<T>();
    }
}