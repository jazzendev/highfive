// import { useParams } from "react-router-dom";
// import queryString from 'query-string';

// function AccountMgmt(props) {
//   const { id } = useParams();
//   const params = queryString.parse(props.location.search);

//   return (
//     <div>
//       <h1>Account Management</h1>
//       <div>{id}</div>
//       <div>{JSON.stringify(params)}</div>
//     </div>
//   );
// }

// export default AccountMgmt;

import Index from './AccountMgmt/Index';

function AccountMgmt() {
    return (
        <Index></Index>
    );
}

export default AccountMgmt;
