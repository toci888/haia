import React, { useEffect, useState } from 'react';
import { getAllComedyTexts } from '../services/comedyTextService';
import './styles/ComedyTextList.module.css';

export interface ComedyText {
    id: number; // Assuming id is a number
    text: string; // Assuming text is a string
}

const ComedyTextList: React.FC = () => {
    const [comedyTexts, setComedyTexts] = useState<ComedyText[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                //const data = await getAllComedyTexts() as ComedyText[];

                //const enrichedData: ComedyText[] = data.map(item => ({
                //    ...item,
                //    text: item.text || 'Brak tekstu' // Uzupełniamy brakujące dane
                //}));
                //setComedyTexts(enrichedData);

            } catch (error) {
                console.error('Failed to fetch comedy texts:', error);
            }
        };
        fetchData();
    }, []);

    return (
        <ul className="list-group">
            {comedyTexts.map((text) => (
                <li key={text.id} className={`list-group-item `}>
                    <span>{text.text}</span>
                </li>
            ))}
        </ul>
    );
};

export default ComedyTextList;