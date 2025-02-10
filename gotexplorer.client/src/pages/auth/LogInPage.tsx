import { Link, Navigate, useNavigate } from "react-router-dom";
import "./Auth.scss";
import { useState } from "react";
import authService from "./authService";
import Cookies from "universal-cookie";

const LogInPage = () => {
    const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%]).{8,24}$/;
    const [userData, setUserData] = useState({
        username: "",
        password: ""
    });
    const navigate = useNavigate();

    const authserv = authService;
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;

    const [isPasswordVisible, setPasswordVisible] = useState(false);
    const [rememberMe, setRememberMe] = useState(false);
    const togglePasswordVisibility = () => {
        setPasswordVisible(!isPasswordVisible);
    };
    const toggleRememberMe = () => {
        setRememberMe(!rememberMe);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setUserData({
            ...userData,
            [e.target.name]: value
        });
    };
    function Submit() {
        const passValid = PWD_REGEX.test(userData.password);
        if (passValid) {
            authserv.login(userData.username, userData.password)
                .then((r) => {
                    console.log("Login successful:", r);
                    navigate("/");
                })
                .catch((error) => {
                    console.error("Login failed:", error);
                });
        }
        else {
            console.log("wrong pass");
        }
    }
    return (<>{isAuthenticated ? <Navigate to="/"></Navigate> :
        <div className="auth-grid">
            <img className="photo-bg" />
            <div className="col-2">
                <img className="logo" />
                <div className="greeting">
                    Nice to see you
                </div>
                <form>
                    <label>Login</label>
                    <div id="email-container">
                        <input
                            id="username"
                            name="username"
                            placeholder="Username"
                            required
                            autoComplete="off"
                            defaultValue={userData.username}
                            onChange={handleChange} />
                    </div>
                    <label>Password</label>
                    <div id="pass-container">
                        <input id="pass"
                            name="password"
                            type={isPasswordVisible ? 'text' : 'password'} placeholder="Password"
                            required
                            autoComplete="off"
                            defaultValue={userData.password}
                            onChange={handleChange} />
                        <button type="button" onClick={togglePasswordVisibility}>
                            <img id="pass-eye-img" />
                        </button>
                    </div>
                    <div id="rmmbr-btn-container">
                        <label className="remember-me">
                            <input
                                type="checkbox"
                                checked={rememberMe}
                                onChange={toggleRememberMe}
                            />
                            Remember me
                        </label>
                        <Link to="/forgot-password" className="link" > Forgot password </Link>
                    </div>

                    <input className="submit-btn" value="Log in" onClick={Submit}></input>
                </form>
                <Link to="/signup" className="link">Don't have an account? Sign up</Link>
            </div>
        </div>
    }
    </>
    );
}
export default LogInPage;