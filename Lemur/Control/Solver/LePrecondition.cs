namespace Lemur.Control.Solver
{
    public enum LePrecondition
    {
        SSOR = 1,
        DiagonalScaling = 3,
        ML_AMG = 5,
        BLOCK_ILU0 = 10,
        BLOCK_ILU1 = 11,
        BLOCK_ILU2 = 12,
    }
}
