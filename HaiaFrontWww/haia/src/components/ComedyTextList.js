import React, { useEffect, useState } from 'react';
import { getAllComedyTexts } from '../services/comedyTextService';

const ComedyTextList = () => {
    const [comedyTexts, setComedyTexts] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getAllComedyTexts();
                setComedyTexts(data);
            } catch (error) {
                console.error('Failed to fetch comedy texts:', error);
            }
        };

        fetchData();
    }, []);

    return (
        <ul className="list-group">
            {comedyTexts.map((text) => (
                <li key={text.id} className="list-group-item comedyTextItem">
                    <span className="textTitle">{text.text}</span>
                </li>
            ))}
        </ul>
    );
};

export default ComedyTextList;
