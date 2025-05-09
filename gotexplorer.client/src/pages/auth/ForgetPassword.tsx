import { useEffect, useState } from "react";
import { Navigate, Link } from "react-router-dom";
import "./Auth.scss";
import Cookies from "universal-cookie";
import authService from "./authService";
import { toast } from "sonner";
import ErrorHandle from "../../utils/ErrorHandle";

const ForgetPasswordPage = () => {
    useEffect(() => {
        document.body.classList.add("auth-body");
        return () => {
            document.body.classList.remove("auth-body");
        };
    }, []);

    const EML_REGEX = /^[\w-\\.]+@([\w-]+\.)+[\w-]{2,4}$/g;
    const [email, setEmail] = useState('');

    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;
    const authserv = authService;

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setEmail(value);
    };

    function Submit() {
        const emailValid = EML_REGEX.test(email);
        if (!emailValid) {
            toast.error("Please enter a valid email");
            return;
        }
        authserv.reset_password(email)
            .then(() => {
                toast.success("Link is sent to your email!");
            })
            .catch((error) => {
                toast.error(`Error sending an email: ${ErrorHandle(error.response.data.errors)}`);
            })
    }
    return (<>{isAuthenticated ? <Navigate to="/profile"></Navigate> :
        <div className="auth-grid">
            <img className="photo-bg" />
            <div className="col-2">
                <Link to="/"><img className="logo" /></Link>
                <div className="greeting">
                    Forgot password
                </div>
                <form>
                    <label>Please enter your email to reset your password</label>
                    <div id="email-container">
                        <input
                            id="email"
                            name="email"
                            placeholder="Enter your email"
                            required
                            autoComplete="off"
                            defaultValue={email}
                            onChange={handleChange} />
                    </div>

                    <input className="submit-btn" type="button" value="Reset password" onClick={Submit}></input>
                </form>
            </div>
        </div>
    }
    </>
    );
}
export default ForgetPasswordPage;