import React from 'react';
import BootstrapTable from 'react-bootstrap-table-next';
import paginationFactory from 'react-bootstrap-table2-paginator';
import "../Style/table.css";

const actionFormatter = (cell, row, rowIndex, columnIndex) => {
    // Your formatting logic goes here
    return <span>{cell}</span>; // Replace with your actual logic
  };

const isMobile = true;

const checkFormatter = (cell, row, rowIndex, columnIndex1, columnIndex2) => {
    // Your formatting logic goes here
    return <span>{cell}</span>; // Replace with your actual logic
  };

  const RemoteAll = ({ data, page, sizePerPage, totalSize, onTableChange }) => (
    <div>
      <BootstrapTable
        remote
        hover
        keyField="id"
        rowEvents={rowEvents}
        data={data}
        columns={columns}
        rowClasses="boostrap-table-row"
        noDataIndication={"Nema podataka za prikaz."}
        pagination={paginationFactory({
          page,
          sizePerPage,
          totalSize,
          hideSizePerPage: true,
        })}
        onTableChange={onTableChange}
      />
    </div>
  );

  const rowEvents = {
    onClick: (e, row, rowIndex) => {
      // Handle row click event
      console.log(`Clicked on row with index ${rowIndex}`);
    },
    // Add more row events as needed
  };
  
const columns = [
    {
      dataField: "id",
      text: "*",
      headerStyle: (colum, colIndex) => {
        return { width: "45px", textAlign: "center", fontSize: "28px" };
      },
      formatter: actionFormatter,
    },
    {
      dataField: "name",
      text: "Naziv izvještaja",
    },
    {
      dataField: "odgovorna osoba",
      text: "odgovorna osoba",
    },
    {
      dataField: "napomena",
      text: "napomena",
    },
    {
      dataField: "zadnji uredio",
      text: "zadnji uredio",
    },
    /*{
      dataField: "dateEdited",
      text: "Datum uređivanja",
      formatter: dateTimeFormatter,
      hidden: Boolean(isMobile),
    },*/
    /*{
      dataField: "dateCreated",
      text: "Datum kreiranja",
      formatter: dateTimeFormatter,
      hidden: Boolean(isMobile),
    },*/
  ];

 
export default RemoteAll;
