import { Outlet } from "react-router-dom";
import Navigation from "./Navigation";
import Footer from "./Footer";

const Layout = () => {
    return (<>
        <div style={{ backgroundColor: "black" }}>
            <Navigation />
        </div>
        <Outlet />
        <Footer />
    </>);
}
export default Layout;