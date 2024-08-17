import React from 'react';

const HandleButtons = ({ handleUpdate, handleDetail, selectedItemsLength }) => (
  <div>
    <button onClick={handleUpdate} disabled={selectedItemsLength === 0}>
      Update
    </button>
    <button onClick={handleDetail} disabled={selectedItemsLength === 0}>
      Detail
    </button>
  </div>
);

export default HandleButtons;
