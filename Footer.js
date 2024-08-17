import React from "react";


const Footer = () => {
    return (
      <footer className="text-center text-white">
        &copy; {new Date().getFullYear()} Središnji državni ured za razvoj
        digitalnog društva.  
        &nbsp;
        <a className="text-center text-white" href="
  https://rdd.gov.hr/uvjeti-koristenja/76
  ">Uvjeti korištenja.</a>
        &nbsp;
        <a className="text-center text-white"  href="
  https://rdd.gov.hr/izjava-o-pristupacnosti/1458
  ">Izjava o pristupačnosti.</a>
        &nbsp;
        <a className="text-center text-white"  href="
  https://ssi.gov.hr/hr/informacije/pravne-napomene/pravne-napomene
   ">Pravne napomene.</a>
      </footer>
      
    );
  };
  
  export default Footer; 