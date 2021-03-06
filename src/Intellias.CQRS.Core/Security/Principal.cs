using System;
using System.Linq;
using System.Security.Claims;

namespace Intellias.CQRS.Core.Security
{
    /// <summary>
    /// Represents a set of Claims and Permissions under which code is executed.
    /// </summary>
    public class Principal
    {
        /// <summary>
        /// Identity id.
        /// </summary>
        public string IdentityId { get; set; } = string.Empty;

        /// <summary>
        /// User id.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// True if Actor is Process Manager.
        /// </summary>
        public bool IsProcessManager { get; set; }

        /// <summary>
        /// Identity claims.
        /// </summary>
        public IdentityClaim[] Claims { get; set; } = Array.Empty<IdentityClaim>();

        /// <summary>
        /// Identity permissions.
        /// </summary>
        public string[] Permissions { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Checks whether Identity has the role.
        /// </summary>
        /// <param name="roleName">Role name.</param>
        /// <returns>True if Identity has the role.</returns>
        public bool IsInRole(string roleName)
        {
            var roleClaim = Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role && string.Equals(c.Value, roleName, StringComparison.OrdinalIgnoreCase));
            return roleClaim != null;
        }

        /// <summary>
        /// Converts <see cref="Principal"/> to <see cref="Actor"/>.
        /// </summary>
        /// <returns>Principal Actor.</returns>
        public Actor AsActor()
        {
            return new Actor
            {
                IdentityId = IdentityId,
                UserId = UserId,
                IsProcessManager = IsProcessManager
            };
        }
    }
}