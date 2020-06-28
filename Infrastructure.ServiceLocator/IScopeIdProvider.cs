// <copyright file="IScopeIdProvider.cs" company="Beckman Coulter">
// Copyright (c) Beckman Coulter. All rights reserved.
// </copyright>

namespace Authoring.Infrastructure.ServiceLocator
{
    /// <summary>
    /// The interface which a class inherit from provides scope id of the service locator.
    /// </summary>
    public interface IScopeIdProvider
    {
        /// <summary>
        /// The scope id of the service locator.
        /// </summary>
        int ScopeId { get; set; }
    }
}