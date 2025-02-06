import { Link } from "react-router-dom";
import "./Auth.scss";
import { useState } from "react";


const LogInPage = () => {
    const [isPasswordVisible, setPasswordVisible] = useState(false);
    const [rememberMe, setRememberMe] = useState(false);
    const togglePasswordVisibility = () => {
        setPasswordVisible(!isPasswordVisible);
    };
    const toggleRememberMe = () => {
        setRememberMe(!rememberMe);
    };
    return (
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
                        <input id="email" placeholder="Email" />
                    </div>
                    <label>Password</label>
                    <div id="pass-container">
                        <input id="pass"
                            type={isPasswordVisible ? 'text' : 'password'} placeholder="Password" />
                        <button type="button" onClick={togglePasswordVisibility}>
                            <img id="pass-eye-img" />
                        </button>
                    </div>
                    <div id="rmmbr-btn-container">
                        <label className = "remember-me">
                            <input
                                type="checkbox"
                                checked={rememberMe}
                                onChange={toggleRememberMe}
                            />
                            Remember me
                        </label>
                        <Link to="/forgot-password" className="link" > Forgot password </Link>
                    </div>

                    <input type="submit" value="Log in"></input>
                </form>
                <Link to="/signup" className="link">Don't have an account? Sign up</Link>
            </div>
        </div>);
}
export default LogInPage;