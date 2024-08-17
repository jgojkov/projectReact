import React, {useState} from "react";
import HandleButtons from "../Layout/HandleButtons";
import Loader from "../Layout/Loader";
import RemoteAll from "../Layout/RemoteAll";
import Card from 'react-bootstrap/Card';
import "../Style/card.css";
import "../Style/menu-dropdown.css";
import "../Style/form.css";



const Home = () => {
  
  const [isLoading, setIsLoading] = useState(false);
  const [search, setSearch] = useState({ searchText: '', active: false });
  const [pagination, setPagination] = useState({
    pageIndex: 0,
    pageSize: 10,
    totalCount: 0,
  });
  const [selectedItems, setSelectedItems] = useState([]);
  const [showEditModal, setShowEditModal] = useState(false);
  const [tenantElements, setTenantElements] = useState(null);
  const [tenants, setTenants] = useState([]);
  const [update, setUpdate] = useState(false);

  // Function definitions
  const handleToUpdate = () => {
    // Your logic for handleToUpdate
  };

  const handleToDetail = () => {
    // Your logic for handleToDetail
  };

  const handleTableChange = () => {
    // Your logic for handleTableChange
  };

 return (
  <Card className="card">
  <Card.Header className="card-header">
    <h5 style={{ fontSize: "0.9rem" }}> 
      Izvještaji
      <span className="badge badge-pill badge-primary">
        {pagination.totalCount}
      </span>
    </h5>
  </Card.Header>
  <Card.Body>
    <>
      <fieldset className="scheduler-border">
        <legend className="scheduler-border">Filteri</legend>
        <form className="row">
          <div className="form-group col-lg-3 col-md-4 col-sm-6 col-xs-12">
            <label>Izvještaj</label>
            <select
              id="noneTextTransform"
              className="form-control"
              onChange={(event) => {
                setIsLoading(true);
                setSearch({ ...search, searchText: event.target.value });
                setPagination({ ...pagination, pageIndex: 0 });
              }}
            />
          </div>
        </form>
      </fieldset>

      <HandleButtons
        handleUpdate={handleToUpdate}
        handleDetail={handleToDetail}
        selectedItemsLength={selectedItems.length}
      />

      <span style={{ fontSize: "10px" }} className="font-weight-light">
        <span style={{ fontSize: "20px" }}>*</span>Odaberite redak kako biste omogućili iznad dostupne akcije
      </span>

      {isLoading ? (
        <Loader />
      ) : (
        <RemoteAll
          data={tenants}
          page={pagination.pageIndex + 1}
          sizePerPage={pagination.pageSize}
          totalSize={pagination.totalCount}
          onTableChange={handleTableChange}
        />
      )}
    </>
  </Card.Body>
</Card>
 )
        };


export default Home; 