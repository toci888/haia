import React, { useEffect, useState } from 'react';
import { getAllComedyTexts } from '../services/comedyTextService';
import styles from './styles/ComedyTextList.module.css';

interface ComedyText {
    id: number; // Assuming id is a number
    text: string; // Assuming text is a string
}

const ComedyTextList: React.FC = () => {
    const [comedyTexts, setComedyTexts] = useState<ComedyText[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getAllComedyTexts();
                setComedyTexts(data as ComedyText[]);
            } catch (error) {
                console.error('Failed to fetch comedy texts:', error);
            }
        };
        fetchData();
    }, []);

    return (
        <ul className="list-group">
            {comedyTexts.map((text) => (
                <li key={text.id} className={`list-group-item ${styles.comedyTextItem}`}>
                    <span className={styles.textTitle}>{text.text}</span>
                </li>
            ))}
        </ul>
    );
};

export default ComedyTextList;