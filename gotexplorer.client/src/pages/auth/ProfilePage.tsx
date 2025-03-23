/* eslint-disable @typescript-eslint/no-explicit-any */
import Footer from '../additional_components/Footer';
import Navigation from '../additional_components/Navigation';
import './ProfilePage.scss';
import Cookies from 'universal-cookie';
import { Navigate, useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { useEffect, useRef, useState } from 'react';
import authService from './authService';

const Popover = ({ onSelectImage, popoverRef }: { onSelectImage: (imageId: string) => void; popoverRef: React.RefObject<HTMLDivElement>; }) => {
    const [children, setChildren] = useState<JSX.Element[]>([]);
    const getImages = async () => {
        const authserv = authService;
        const r = await authserv.get_images();
        r.forEach((item: any) => {
            if (item.name != "") {
                setChildren((prevChildren) => [...prevChildren, <PossiblePicture key={item.id} id={item.id} source={item.path} onSelect={onSelectImage} />]);

            }
        })
    }
    useEffect(() => {
        getImages();
    }, [])
    return (<div id="popover" className="popover" ref={popoverRef}>
        {children}
    </div>);
}
const PossiblePicture = ({ id, source, onSelect }: { id: string, source: string, onSelect: (id: string) => void }) => {
    const cookies = new Cookies();
    const handleMouseEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        cookies.set("changedImage", (e.target as Element).id);
        onSelect(id);
    };

    return <button onClick={handleMouseEvent}>
        <img id={id} src={source} />
    </button>
}
const ProfilePage = () => {
    const cookies = new Cookies();
    const token = cookies.get('token');
    const isAuthenticated = token != null ? true : false;
    const authserv = authService;
    const navigate = useNavigate();
    const [userData, setUserData] = useState({
        name: "",
        email: ""
    });
    const [changedName, setChangedName] = useState("");
    const [changedPasswords, setChangedPasswords] = useState({
        oldPassword: "",
        changedPassword: ""
    });
    const [imageSrc, setImageSrc] = useState<string | undefined>(undefined);
    const [popover, setPopover] = useState(false);
    const popoverRef = useRef<HTMLDivElement>(null);

    const handleClickOutside = (event: MouseEvent) => {
        if (popoverRef.current && !popoverRef.current.contains(event.target as Node)) {
            setPopover(false); 
        }
    };

    useEffect(() => {
        document.addEventListener("mousedown", handleClickOutside);
        try {
            setUserData(jwtDecode(token));
        } catch (error) {
            console.error('Token decoding failed:', error);
        }
        const fetchImage = () => {
            try {
                if (!cookies.get("changedImage")) {
                    authserv.get_image(cookies.get("imageId")).
                        then((resp) => {
                            setImageSrc(resp);
                        });
                }
                else {
                    authserv.get_image(cookies.get("changedImage")).
                        then((resp) => {
                            setImageSrc(resp);
                        });
                }
            } catch (error) {
                console.error("Error fetching image:", error);
            }
        };

        fetchImage();
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
            if (cookies.get("changedImage") != undefined && cookies.get("changedImage") != cookies.get("imageId")) {
                try {
                    authserv.update_photo();
                }
                catch (error) {
                    console.error(error);
                }
            }
        }
    }, []);

    const handleImageSelect = (imageId: string) => {
        try {
            authserv.get_image(imageId).
                then((resp) => {
                    setImageSrc(resp);
                });
        } catch (error) {
            console.error("Error fetching image:", error);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setChangedPasswords({
            ...changedPasswords,
            [e.target.name]: value
        });
    };
    const handlePopout = () => {
        setPopover(!popover);
    }
    function changeName() {
        if (changedName != "") {
            authserv.update(changedName, userData.email)
                .catch((err) => {
                    console.error(err);
                });
        }
    }
    function changePassword() {
        if (changedPasswords.oldPassword != "" && changedPasswords.changedPassword != "") {
            authserv.update_password(changedPasswords.oldPassword, changedPasswords.changedPassword).then(() => { navigate(0); })
                .catch((err) => {
                    console.error(err);
                });
        }
    }
    function deleteProfile() {
        try {
            authserv.delete();
            navigate("/");
        }
        catch (error) {
            console.log(error);
        }
    }
    function logOut() {
        authserv.logout();
        navigate("/");
    }
    return (<>{isAuthenticated ?
        <div className="profile-page">
            <Navigation />
            <div className="profile-container">
                <div className="email">
                    {userData ? (
                        <p>{userData.email}</p>
                    )
                        : (
                            <p>No email</p>
                        )
                    }
                </div>
                <div className="profile-header">
                    <div className="profile-picture-wrapper popover-container">
                        <img
                            className="profile-picture"
                            src={imageSrc}
                            alt="Profile"
                        />
                        {
                            popover &&
                            <Popover onSelectImage={handleImageSelect} popoverRef={popoverRef} />
                        }
                        <button className="change-picture-button" onClick={handlePopout}>📸</button>
                    </div>
                    <h2 className="username">{userData.name}</h2>
                </div>

                <div className="profile-leaderboard">
                    <h4>Leader board</h4>
                    <p>
                        1. User23 - 1984 points
                        <br />
                        34. You - 578 points
                    </p>
                </div>


                <div className="profile-form">
                    <div className="form-group">
                        <label htmlFor="changedName">Name</label>
                        <input id="changedName" name="changedName" type="text" placeholder="Change your name" onChange={(e) => setChangedName(e.target.value)} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                        <button className="save-button" onClick={changeName}>Save new username</button>
                    </div>
                    <hr />
                    <div className="form-group">
                        <label htmlFor="oldPassword">Old Password</label>
                        <input id="oldPassword" name="oldPassword" type="password" placeholder="Enter old password" onChange={handleChange} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                        <label htmlFor="changedPassword">New Password</label>
                        <input id="changedPassword" name="changedPassword" type="password" placeholder="Enter new password" onChange={handleChange} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                        <button className="save-button" onClick={changePassword}>Save new password</button>
                    </div>
                    <div className="two-buttons">
                        <button className="logout-button" onClick={logOut}>Log out</button>
                        <button className="delete-button" onClick={deleteProfile}>Delete account</button>
                    </div>
                </div>
            </div>
            <Footer />
        </div>
        : <Navigate to="/login"></Navigate>
    }
    </>
    );
}
export default ProfilePage;