import { useEffect, useState } from "react";
import "./MainPage.scss";
import "aos/dist/aos.css";
import AOS from 'aos';

const InfoSection = () => {
    const [activeInfo, setActiveInfo] = useState("eyerie");

    const handleButtonClick = (info: string) => {
        setActiveInfo(info);
    };


    useEffect(() => {
        AOS.init();
        AOS.refresh();
    },);

    return (<div id="info_sec">
        <div className="col-1" data-aos="fade-up" data-aos-duration="800" data-aos-easing="ease-in-sine" data-aos-once="true">
            <button onClick={() => handleButtonClick("eyerie")}>
                <img id="eyrie-logo" />
            </button>
            <button onClick={() => handleButtonClick("dragonstone")}>
                <img id="dragonstone-logo" />
            </button>
            <button onClick={() => handleButtonClick("kingslanding")}>
                <img id="kingslanding-logo" />
            </button>
        </div>
        <div className="col-2" data-aos="fade-up" data-aos-duration="800" data-aos-easing="ease-in-sine" data-aos-once="true">
            {activeInfo === "eyerie" && (
            <div className="info fade">
            <h3>THE EYRIE</h3>
                <p>The Eyrie is the principal stronghold of House Arryn. It is located in the Vale of Arryn near the east coast of Westeros. The Eyrie straddles the top of a peak in the Mountains of the Moon several thousand feet above the valley floor below. It is approached by a narrow causeway and road.</p>
            </div>
            )}
            {activeInfo === "dragonstone" && (
                <div className="info fade">
                    <h3>DRAGONSTONE</h3>
                    <p>Dragonstone, or Dragonstone Castle, is the castle that stands upon the eponymous island located in Blackwater Bay. It is the ancestral seat of House Targaryen and the former stronghold of House Baratheon of Dragonstone, a cadet branch of House Baratheon of Storm's End. It is within the Crownlands, the capital region of the Seven Kingdoms.</p>
                </div>
            )}
            {activeInfo === "kingslanding" && (
                <div className="info fade">
                    <h3>KING'S LANDING</h3>
                    <p>King's Landing is the capital, and largest city, of the Six Kingdoms. Located on the east coast of Westeros in the Crownlands, just north of where the Blackwater Rush flows into Blackwater Bay and overlooking Blackwater Bay, King's Landing is the site of the Iron Throne and the Red Keep, the seat of the King of the Andals and the First Men.</p>
                </div>
            )}
        </div>
    </div>);
}
export default InfoSection;