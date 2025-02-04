import React from "react";
import { createBrowserRouter, RouterProvider, Outlet } from "react-router-dom";
import { NavigationComponent } from "./components/NavigationComponent";
import { AboutPage } from "./pages/About";
import { HomePage } from "./pages/Home";
// loader definition outside UserEditScreen component
/*
export const loader = (msalInstance: PublicClientApplication) => 
  ({ request, params }: LoaderFunctionArgs) => {
    ...
    // some more code to clarify my need on the msal instance inside the loader
    const request = {
      account: msalInstance.getActiveAccount(),
      scopes: scopes
    }
    const accessToken = await msalInstance.acquireTokenSilent(request)
      .then(response => response.accessToken)
      .catch(error => {        
        console.error(error);
      })
    // then with the accessToken I can just use made 
    // a javascript fetch request to my secured endpoint in azure

    ...
  };
*/
const createAppRouter = () => createBrowserRouter([
    {
        path: "/",
        element: <Layout />,
        children: [
            {
                index: true,
                element: <HomePage />
            },            
            {
                path: "about",
                element: <AboutPage />
            }
        ]
    }
]);

function Layout() {
    return (
        <div className="container-fluid">
            <NavigationComponent />
            <Outlet />
        </div>
    );
}

export function App() {
    return (
        <>
            <RouterProvider router={createAppRouter()} />
        </>
    );
}
