using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace HotChocolate.Execution.Utilities
{
    /// <summary>
    /// A prepared operations is an already compiled and optimized variant 
    /// of the operation specified in the query document that was provided 
    /// in the request.
    /// </summary>
    public interface IPreparedOperation : IOperation
    {
        /// <summary>
        /// Gets the internal unique identifier for this operation.
        /// </summary>
        string Id { get; }

        int ProposedTaskCount { get; }

        /// <summary>
        /// Gets the prepared root selections for this operation.
        /// </summary>
        /// <param name="selectionSet">
        /// The selection set for which the selections shall be collected.
        /// </param>
        /// <param name="typeContext">
        /// The type context under which the selections shall be collected.
        /// </param>
        /// <returns>
        /// Returns the prepared root selections for this operation.
        /// </returns>
        IPreparedSelectionList GetRootSelections();

        /// <summary>
        /// Gets the prepared selections for the specified 
        /// selection set under the specified type context.
        /// </summary>
        /// <param name="selectionSet">
        /// The selection set for which the selections shall be collected.
        /// </param>
        /// <param name="typeContext">
        /// The type context under which the selections shall be collected.
        /// </param>
        /// <returns>
        /// Returns the prepared selections for the specified 
        /// selection set under the specified type context.
        /// </returns>
        IPreparedSelectionList GetSelections(
            SelectionSetNode selectionSet,
            ObjectType typeContext);

        /// <summary>
        /// Prints the prepared operation. 
        /// </summary>
        string Print();
    }
}
