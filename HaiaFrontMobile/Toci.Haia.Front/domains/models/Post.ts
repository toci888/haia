import { Category } from "./Category";
import { Group } from "./Group";

export interface Post {
  id: number;
  createdAt: string;
  content: string;
  userId: number;
  user: {
    id: number;
    username: string;
  }
  groupId: number;
  group: Group;
  categoryId: number;
  category: Category;
  comments: any[];
}