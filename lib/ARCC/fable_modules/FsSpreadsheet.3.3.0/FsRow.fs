﻿namespace FsSpreadsheet

open System.Collections.Generic
open System.Collections

open Fable.Core
// Type based on the type XLRow used in ClosedXml
/// <summary>
/// Creates an FsRow from the given FsRangeAddress, consisting of FsCells within a given FsCellsCollection, and a styleValue.
/// </summary>
/// <remarks>The FsCellsCollection must only cover 1 row!</remarks>
/// <exception cref="System.Exception">if given FsCellsCollection has more than 1 row.</exception>
[<AttachMembers>]
type FsRow (rangeAddress : FsRangeAddress, cells : FsCellsCollection)= 

    inherit FsRangeBase(rangeAddress)

    let cells = cells

    // ----------
    // Creation
    // ----------

    static member empty() = FsRow (FsRangeAddress(FsAddress(0,0),FsAddress(0,0)),FsCellsCollection())

    /// <summary>
    /// Create an FsRow from a given FsCellsCollection and an rowIndex.
    /// </summary>
    /// <remarks>The appropriate range of the cells (i.e. minimum colIndex and maximum colIndex) is derived from the FsCells with the matching rowIndex.</remarks>
    static member createAt(index, (cells : FsCellsCollection)) = 
        let getIndexBy (f : (FsCell -> int) -> seq<FsCell> -> FsCell) = 
            match cells.GetCellsInRow index |> Seq.length with
            | 0 -> 1
            | _ ->
                (
                    cells.GetCellsInRow index 
                    |> f (fun c -> c.Address.ColumnNumber)
                ).Address.ColumnNumber
        let minColIndex = getIndexBy Seq.minBy
        let maxColIndex = getIndexBy Seq.maxBy
        FsRow (FsRangeAddress(FsAddress(index, minColIndex),FsAddress(index, maxColIndex)), cells)

    interface IEnumerable<FsCell> with
        member this.GetEnumerator() : System.Collections.Generic.IEnumerator<FsCell> = this.Cells.GetEnumerator()

    interface IEnumerable with
        member this.GetEnumerator() = (this :> IEnumerable<FsCell>).GetEnumerator() :> IEnumerator

    // ----------
    // PROPERTIES
    // ----------

    /// The associated FsCells.
    member self.Cells = 
        base.Cells(cells)

    /// <summary>
    /// The index of the FsRow.
    /// </summary>
    member self.Index 
        with get() = self.RangeAddress.FirstAddress.RowNumber
        and set(i) = 
            self.RangeAddress.FirstAddress.RowNumber <- i
            self.RangeAddress.LastAddress.RowNumber <- i


    // -------
    // METHODS
    // -------

    /// <summary>
    /// Creates a deep copy of this FsRow.
    /// </summary>
    member self.Copy() =
        let ra = self.RangeAddress.Copy()
        let cells = self.Cells |> Seq.map (fun c -> c.Copy())
        let fcc = FsCellsCollection()
        fcc.Add cells
        FsRow(ra, fcc)

    /// <summary>
    /// Returns a deep copy of a given FsRow.
    /// </summary>
    static member copy (row : FsRow) =
        row.Copy()

    /// <summary>
    /// Returns the index of the given FsRow.
    /// </summary>
    static member getIndex (row : FsRow) = 
        row.Index
       
    /// <summary>
    /// Returns the FsCell at columnIndex.
    /// </summary>
    member this.Item (columnIndex) =
        // use FsRangeBase call with colindex 1
        base.Cell(FsAddress(1,columnIndex),cells)       

    /// <summary>
    /// Returns the FsCell at the given columnIndex from an FsRow.
    /// </summary>
    static member item colIndex (row : FsRow) =
        row.Item(colIndex)

    /// <summary>
    /// Inserts the value at columnIndex as an FsCell. If there is an FsCell at the position, this FsCells and all the ones right to it are shifted to the right.
    /// </summary>
    member this.InsertValueAt(colIndex, (value : 'a)) =
        let cell = FsCell(value)
        cells.Add(int32 this.Index, int32 colIndex, cell)

    /// <summary>
    /// Adds a value at the given row- and columnIndex to FsRow using.
    ///
    /// If a cell exists in the given position, shoves it to the right.
    /// </summary>
    static member insertValueAt colIndex value (row : FsRow) =
        row.InsertValueAt(colIndex, value) |> ignore
        row

    //member self.SortCells() = _cells <- _cells |> List.sortBy (fun c -> c.WorksheetColumn)

    // TO DO (later)
    ///// Takes an FsCellsCollection and creates an FsRow from the given rowIndex and the cells in the FsCellsCollection that share the same rowIndex.
    //static member fromCellsCollection rowIndex (cellsCollection : FsCellsCollection) =