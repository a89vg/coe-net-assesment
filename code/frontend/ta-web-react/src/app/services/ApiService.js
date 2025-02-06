import axios from "axios";

const BASE_URL = import.meta.env.VITE_APIURL ?? "/";

const apiClient = axios.create({
  baseURL: BASE_URL
})

export default apiClient;

// export const ApiService = {
//   async testApi(accessToken) {    
//     const res = await fetch(`${BASE_URL}api/test`, {
//       headers: {
//         "Authorization": `Bearer ${accessToken}`
//       }
//     });
//     console.info(res);
//     if (res.ok == true) {
//       return await res.json();
//     }
//   }
// }
