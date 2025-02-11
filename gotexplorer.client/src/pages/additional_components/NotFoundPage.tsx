import { useNavigate } from "react-router-dom";
import Navigation from "./Navigation";
import "./NotFoundPage.scss";
import { useEffect } from "react";
const NotFoundPage = () => {
    const navigate = useNavigate();
    const handleClick = () => {
        navigate('/');
    }
    useEffect(() => {
        document.body.classList.add("nf-body");
        return () => {
            document.body.classList.remove("nf-body");
        };
    }, []);
    return (<>
        <Navigation />
        <div className="nf-container">
            <h2>
                OOOPS
            </h2>
            <h3>
                PAGE NOT FOUND
            </h3>
            <h1>
                404
            </h1>
            <button onClick={handleClick}>
                Go to homepage
            </button>
        </div>
    </>
    );
};
export default NotFoundPage;
