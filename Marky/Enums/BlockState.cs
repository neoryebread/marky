namespace Marky;

/// <summary>
/// Represents the current block-level state of the parser.
/// </summary>
internal enum BlockState
{
    None,
    InList,
    InBlockquote
}
